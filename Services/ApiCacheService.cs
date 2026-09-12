using System.Collections.Concurrent;
using System.Diagnostics;
using AirIQ.Models;
using AirIQ.Services.Interfaces;
using Newtonsoft.Json;
using Sentry;
using SQLite;

namespace AirIQ.Services;

/// <summary>
/// SQLite-backed implementation of <see cref="IApiCacheService"/>. Reusable across every Refit API:
/// callers only need a unique cache key and a delegate that invokes the underlying API method.
///
/// Rate-limit/cache state (last response, next-allowed-call time, call history) lives in SQLite so it
/// survives app restarts. A per-cache-key <see cref="SemaphoreSlim"/> serializes concurrent callers for
/// the same key so only one live API call is ever made even if multiple pages request the same data at
/// once; the SQLite write that reserves a call slot happens inside a single transaction so the persisted
/// state itself is never left half-updated.
/// </summary>
public class ApiCacheService : IApiCacheService, IDisposable
{
    private static readonly TimeSpan DefaultMinCallInterval = TimeSpan.FromHours(1);
    private static readonly TimeSpan DefaultRollingWindow = TimeSpan.FromHours(24);
    private const int DefaultMaxCallsPerRollingWindow = 24;
    private const string DatabaseFileName = "airiq_apicache.db3";

    private readonly SQLiteAsyncConnection _connection;
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _keyLocks = new();
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private volatile bool _isInitialized;

    public ApiCacheService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);
        _connection = new SQLiteAsyncConnection(dbPath);
    }

    public async Task<T?> GetAsync<T>(
        string cacheKey,
        Func<Task<T?>> apiCall,
        CancellationToken cancellationToken = default,
        TimeSpan? minCallInterval = null,
        int? maxCallsPerRollingWindow = null,
        TimeSpan? rollingWindow = null) where T : class
    {
        if (string.IsNullOrWhiteSpace(cacheKey))
        {
            throw new ArgumentException("Cache key must not be empty.", nameof(cacheKey));
        }

        ArgumentNullException.ThrowIfNull(apiCall);

        var interval = minCallInterval ?? DefaultMinCallInterval;
        var window = rollingWindow ?? DefaultRollingWindow;
        var maxCalls = maxCallsPerRollingWindow ?? DefaultMaxCallsPerRollingWindow;

        await EnsureInitializedAsync().ConfigureAwait(false);

        var keyLock = _keyLocks.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));
        await keyLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var now = DateTime.UtcNow;
            var cacheEntry = await GetCacheEntryAsync(cacheKey).ConfigureAwait(false);
            var callsInWindow = await CountCallsInWindowAsync(cacheKey, now - window).ConfigureAwait(false);

            if (callsInWindow >= maxCalls)
            {
                Debug.WriteLine($"[ApiCacheService] '{cacheKey}': blocked, 24h quota reached ({callsInWindow}/{maxCalls}). Returning cached data.");
                return TryDeserialize<T>(cacheEntry);
            }

            if (cacheEntry != null && now < cacheEntry.NextAllowedCallUtc)
            {
                Debug.WriteLine($"[ApiCacheService] '{cacheKey}': blocked, next call not allowed until {cacheEntry.NextAllowedCallUtc:O}. Returning cached data.");
                return TryDeserialize<T>(cacheEntry);
            }

            Debug.WriteLine($"[ApiCacheService] '{cacheKey}': cache miss/expired, quota available ({callsInWindow}/{maxCalls}). Reserving call slot.");

            var nextAllowedCallUtc = now.Add(interval);
            var historyRetention = window + TimeSpan.FromHours(1);

            // Reserve the slot and record the attempt atomically, before the API is actually called.
            // This way a crash mid-call, or a failed call the provider still counts against quota,
            // can never let a retry burst past the configured limits.
            await ReserveCallSlotAsync(cacheKey, now, nextAllowedCallUtc, historyRetention).ConfigureAwait(false);

            try
            {
                Debug.WriteLine($"[ApiCacheService] '{cacheKey}': calling API.");
                var result = await apiCall().ConfigureAwait(false);

                if (result != null)
                {
                    await SaveCacheAsync(cacheKey, result, now, nextAllowedCallUtc).ConfigureAwait(false);
                    Debug.WriteLine($"[ApiCacheService] '{cacheKey}': API call succeeded, cache updated.");
                    return result;
                }

                Debug.WriteLine($"[ApiCacheService] '{cacheKey}': API call returned an empty response. Returning cached data, if any.");
                return TryDeserialize<T>(cacheEntry);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                Debug.WriteLine($"[ApiCacheService] '{cacheKey}': API call failed ({ex.GetType().Name}). Returning cached data, if any.");
                return TryDeserialize<T>(cacheEntry);
            }
        }
        finally
        {
            keyLock.Release();
        }
    }

    private async Task EnsureInitializedAsync()
    {
        if (_isInitialized)
        {
            return;
        }

        await _initLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (_isInitialized)
            {
                return;
            }

            await _connection.CreateTableAsync<ApiCacheEntity>().ConfigureAwait(false);
            await _connection.CreateTableAsync<ApiCallHistoryEntity>().ConfigureAwait(false);
            _isInitialized = true;
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
            Debug.WriteLine($"[ApiCacheService] Failed to initialize SQLite tables: {ex}");
            throw;
        }
        finally
        {
            _initLock.Release();
        }
    }

    private async Task<ApiCacheEntity?> GetCacheEntryAsync(string cacheKey)
    {
        try
        {
            return await _connection.Table<ApiCacheEntity>()
                .Where(x => x.CacheKey == cacheKey)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
            Debug.WriteLine($"[ApiCacheService] '{cacheKey}': failed to read cache entry: {ex}");
            return null;
        }
    }

    private async Task<int> CountCallsInWindowAsync(string cacheKey, DateTime sinceUtc)
    {
        try
        {
            return await _connection.Table<ApiCallHistoryEntity>()
                .Where(x => x.CacheKey == cacheKey && x.CalledAtUtc >= sinceUtc)
                .CountAsync()
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
            Debug.WriteLine($"[ApiCacheService] '{cacheKey}': failed to read call history: {ex}");

            // Fail closed: if we can't verify the quota, assume it is exhausted rather than
            // risk calling an API that has a hard, provider-enforced cap.
            return int.MaxValue;
        }
    }

    private Task ReserveCallSlotAsync(string cacheKey, DateTime calledAtUtc, DateTime nextAllowedCallUtc, TimeSpan historyRetention)
    {
        return _connection.RunInTransactionAsync(connection =>
        {
            connection.Insert(new ApiCallHistoryEntity
            {
                CacheKey = cacheKey,
                CalledAtUtc = calledAtUtc
            });

            var existing = connection.Table<ApiCacheEntity>().FirstOrDefault(x => x.CacheKey == cacheKey);
            if (existing != null)
            {
                existing.NextAllowedCallUtc = nextAllowedCallUtc;
                connection.Update(existing);
            }
            else
            {
                connection.Insert(new ApiCacheEntity
                {
                    CacheKey = cacheKey,
                    ResponseJson = string.Empty,
                    LastUpdatedUtc = calledAtUtc,
                    NextAllowedCallUtc = nextAllowedCallUtc
                });
            }

            var cutoffUtc = calledAtUtc - historyRetention;
            connection.Execute("DELETE FROM ApiCallHistory WHERE CacheKey = ? AND CalledAtUtc < ?", cacheKey, cutoffUtc);
        });
    }

    private Task SaveCacheAsync<T>(string cacheKey, T data, DateTime updatedAtUtc, DateTime nextAllowedCallUtc) where T : class
    {
        var json = JsonConvert.SerializeObject(data);
        return _connection.RunInTransactionAsync(connection =>
        {
            var existing = connection.Table<ApiCacheEntity>().FirstOrDefault(x => x.CacheKey == cacheKey);
            if (existing != null)
            {
                existing.ResponseJson = json;
                existing.LastUpdatedUtc = updatedAtUtc;
                existing.NextAllowedCallUtc = nextAllowedCallUtc;
                connection.Update(existing);
            }
            else
            {
                connection.Insert(new ApiCacheEntity
                {
                    CacheKey = cacheKey,
                    ResponseJson = json,
                    LastUpdatedUtc = updatedAtUtc,
                    NextAllowedCallUtc = nextAllowedCallUtc
                });
            }
        });
    }

    private T? TryDeserialize<T>(ApiCacheEntity? entry) where T : class
    {
        if (entry == null || string.IsNullOrWhiteSpace(entry.ResponseJson))
        {
            return null;
        }

        try
        {
            return JsonConvert.DeserializeObject<T>(entry.ResponseJson);
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
            Debug.WriteLine($"[ApiCacheService] '{entry.CacheKey}': cached JSON is corrupted, discarding: {ex}");
            return null;
        }
    }

    public void Dispose()
    {
        foreach (var semaphore in _keyLocks.Values)
        {
            semaphore.Dispose();
        }

        _initLock.Dispose();
        _connection.CloseAsync().GetAwaiter().GetResult();
        GC.SuppressFinalize(this);
    }
}
