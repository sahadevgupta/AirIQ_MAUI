namespace AirIQ.Constants;

/// <summary>
/// Cache keys used with <see cref="AirIQ.Services.Interfaces.IApiCacheService"/>.
/// Each key identifies an independently rate-limited/cached API in SQLite.
/// </summary>
public static class ApiCacheKeys
{
    public const string LookupCountries = nameof(LookupCountries);
    public const string LookupStates = nameof(LookupStates);
    public const string FlightSectors = nameof(FlightSectors);

    private const string FlightSearchPrefix = "FlightSearch";

    /// <summary>
    /// Builds a cache key scoped to one distinct flight search (route/date/passengers/airline).
    /// Different searches get independent cache/rate-limit buckets so caching one search
    /// never blocks or serves stale results for another.
    /// </summary>
    public static string FlightSearch(string? origin, string? destination, string? departureDate,
        string? returnDate, int adult, int child, int infant, string? airlineCode)
        => $"{FlightSearchPrefix}:{origin}:{destination}:{departureDate}:{returnDate}:{adult}:{child}:{infant}:{airlineCode}";
}
