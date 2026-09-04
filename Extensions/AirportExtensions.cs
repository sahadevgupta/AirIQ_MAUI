using System.Text.Json;

using AirIQ.Models;

namespace AirIQ.Extensions
{
    public static class AirportExtensions
    {
        private static List<Airport>? _airports;
        private static Dictionary<string, Airport>? _airportsByIata;

        /// <summary>
        /// Load airports.json from Resources/Raw.
        /// Call once during app startup.
        /// </summary>
        public static async Task InitializeAsync()
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
        public static Airport? GetAirportByCity(this string city)
        {
            if (_airports == null)
                throw new InvalidOperationException("AirportExtensions.InitializeAsync() must be called first.");

            return _airports.FirstOrDefault(x =>
                    string.Equals(x.City, city, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(x.Iata));
        }

        /// <summary>
        /// Returns all airports for a city.
        /// </summary>
        public static List<Airport> GetAirportsByCity(this string city)
        {
            if (_airports == null)
                throw new InvalidOperationException("AirportExtensions.InitializeAsync() must be called first.");

            return _airports
                .Where(x => string.Equals(x.City, city, StringComparison.OrdinalIgnoreCase))
                .ToList();
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
        public static Airport? GetAirportByIata(this string? iataCode)
        {
            if (_airportsByIata == null)
                throw new InvalidOperationException("AirportExtensions.InitializeAsync() must be called first.");

            if (string.IsNullOrWhiteSpace(iataCode))
                return null;

            return _airportsByIata.GetValueOrDefault(iataCode);
        }
    }
}