using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record CountryDto : LookupBaseDto
    {
        [JsonPropertyName("CountryID")]
        public int CountryId { get; set; }

        [JsonPropertyName("CountryName")]
        public string CountryName { get; set; } = string.Empty;
    }
}