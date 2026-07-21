using System.Text.Json.Serialization;

namespace AirIQ.Models
{
    public class Airport
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("iata")]
        public string? Iata { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("icao")]
        public string? Icao { get; set; }

        [JsonPropertyName("longitude")]
        public decimal Longitude { get; set; }

        [JsonPropertyName("latitude")]
        public decimal Latitude { get; set; }
    }
}