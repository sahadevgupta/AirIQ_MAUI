namespace AirIQ.Services.Interfaces;

/// <summary>
/// Generic, reusable SQLite-backed response cache and call-rate limiter for Refit API calls.
/// Persists cached responses and call history so state survives app restarts.
/// </summary>
public interface IApiCacheService
{
    /// <summary>
    /// Returns cached data for <paramref name="cacheKey"/> when it is still fresh or the caller
    /// is currently rate-limited; otherwise invokes <paramref name="apiCall"/>, persists the result,
    /// and returns it. Falls back to the last cached value if <paramref name="apiCall"/> throws.
    /// </summary>
    /// <param name="cacheKey">Unique key identifying the API/response being cached.</param>
    /// <param name="apiCall">Invokes the underlying Refit API method.</param>
    /// <param name="cancellationToken">Propagated to the caller; not swallowed on cancellation.</param>
    /// <param name="minCallInterval">Minimum time between live calls for this key. Defaults to 1 hour.</param>
    /// <param name="maxCallsPerRollingWindow">Maximum live calls allowed within <paramref name="rollingWindow"/>. Defaults to 24.</param>
    /// <param name="rollingWindow">Rolling window over which <paramref name="maxCallsPerRollingWindow"/> is enforced. Defaults to 24 hours.</param>
    Task<T?> GetAsync<T>(
        string cacheKey,
        Func<Task<T?>> apiCall,
        CancellationToken cancellationToken = default,
        TimeSpan? minCallInterval = null,
        int? maxCallsPerRollingWindow = null,
        TimeSpan? rollingWindow = null) where T : class;
}
