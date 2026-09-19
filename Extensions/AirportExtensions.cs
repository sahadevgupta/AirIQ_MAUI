using System.Text.Json;

using AirIQ.Models;

namespace AirIQ.Extensions
{
    public static class AirportExtensions
    {
        private static List<Airport>? _airports;
        private static Dictionary<string, Airport>? _airportsByIata;
        private static Task? _initializeTask;
        private static readonly object _initializeLock = new();

        /// <summary>
        /// Load airports.json from Resources/Raw.
        /// Call once during app startup. Safe to call multiple times/concurrently;
        /// lookups below await this same task so they never race the initial load.
        /// </summary>
        public static Task InitializeAsync()
        {
            lock (_initializeLock)
            {
                _initializeTask ??= InitializeCoreAsync();
                return _initializeTask;
            }
        }

        private static async Task InitializeCoreAsync()
        {
            if (_airports != null)
                return;

            using var stream = await FileSystem.OpenAppPackageFileAsync("airports.json");

            _airports = await JsonSerializer.DeserializeAsync<List<Airport>>(stream)
                         ?? new List<Airport>();

            // Indexed once so IATA lookups (used on every Airport Search Page navigation) are O(1)
            // instead of scanning the full ~7.7k-entry list per lookup.
            _airportsByIata = new Dictionary<string, Airport>(StringComparer.OrdinalIgnoreCase);
            foreach (var airport in _airports)
            {
                if (!string.IsNullOrEmpty(airport.Iata))
                    _airportsByIata.TryAdd(airport.Iata, airport);
            }
        }

        /// <summary>
        /// Returns the first airport matching the city.
        /// </summary>
        public static async Task<Airport?> GetAirportByCityAsync(this string city)
        {
            await InitializeAsync();

            return _airports?.FirstOrDefault(x =>
                    string.Equals(x.City, city, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(x.Iata));
        }

        /// <summary>
        /// Returns the first airport matching the city.
        /// </summary>
        public static Airport? GetAirportByCity(this string city)
        {
            return city.GetAirportByCityAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns all airports for a city.
        /// </summary>
        public static async Task<List<Airport>> GetAirportsByCityAsync(this string city)
        {
            await InitializeAsync();

            return (_airports ?? new List<Airport>())
                .Where(x => string.Equals(x.City, city, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Returns all airports for a city.
        /// </summary>
        public static List<Airport> GetAirportsByCity(this string city)
        {
            return city.GetAirportsByCityAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns IATA code for the first airport in a city.
        /// </summary>
        public static string? GetAirportCode(this string city)
        {
            return city.GetAirportByCity()?.Iata;
        }

        /// <summary>
        /// Returns country from the Iata Code.
        /// </summary>
        public static string? GetCountry(this string iataCode)
        {
            return iataCode.GetAirportByIata()?.Country;
        }

        /// <summary>
        /// Returns the airport matching the Iata Code.
        /// </summary>
        public static async Task<Airport?> GetAirportByIataAsync(this string? iataCode)
        {
            if (string.IsNullOrWhiteSpace(iataCode))
                return null;

            await InitializeAsync();

            return _airportsByIata?.GetValueOrDefault(iataCode);
        }

        /// <summary>
        /// Returns the airport matching the Iata Code.
        /// </summary>
        public static Airport? GetAirportByIata(this string? iataCode)
        {
            return iataCode.GetAirportByIataAsync().GetAwaiter().GetResult();
        }
    }
}