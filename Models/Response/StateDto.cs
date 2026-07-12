using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record StateDto : LookupBaseDto
    {
        [JsonPropertyName("State")]
        public string StateName { get; set; } = string.Empty;

        [JsonPropertyName("IsCountry")]
        public string IsCountry { get; set; } = string.Empty;

        [JsonPropertyName("CountryID")]
        public int CountryId { get; set; }

        [JsonPropertyName("Account1")]
        public List<object> Account1 { get; set; } = [];

        [JsonPropertyName("CityEntry")]
        public List<CityDto> CityEntry { get; set; } = [];

        [JsonPropertyName("CityEntryMain")]
        public List<MainCityDto> CityEntryMain { get; set; } = [];

        [JsonPropertyName("CountryEntry")]
        public CountryDto? CountryEntry { get; set; }

        [JsonPropertyName("District")]
        public List<DistrictDto> District { get; set; } = [];
    }
}