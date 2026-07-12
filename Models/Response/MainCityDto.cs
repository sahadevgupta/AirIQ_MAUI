using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record MainCityDto : LookupBaseDto
    {
        [JsonPropertyName("CityEntryMainID")]
        public int CityEntryMainId { get; set; }

        [JsonPropertyName("CityName")]
        public string CityName { get; set; } = string.Empty;

        [JsonPropertyName("State")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("DistrictID")]
        public int DistrictId { get; set; }

        [JsonPropertyName("District")]
        public DistrictDto? District { get; set; }
    }
}