using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record DistrictDto : LookupBaseDto
    {
        [JsonPropertyName("DistrictID")]
        public int DistrictId { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("State")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("CityEntryMain")]
        public List<MainCityDto> CityEntryMain { get; set; } = [];
    }
}