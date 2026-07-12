using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record CityDto : LookupBaseDto
    {
        [JsonPropertyName("CityID")]
        public int CityId { get; set; }

        [JsonPropertyName("CityName")]
        public string CityName { get; set; } = string.Empty;

        [JsonPropertyName("State")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("PromoCityID")]
        public int? PromoCityId { get; set; }

        [JsonPropertyName("APIVendor")]
        public List<object> ApiVendor { get; set; } = [];

        [JsonPropertyName("APIVendor1")]
        public List<object> ApiVendor1 { get; set; } = [];

        [JsonPropertyName("FDestination")]
        public List<object> FDestination { get; set; } = [];

        [JsonPropertyName("Notification")]
        public List<object> Notification { get; set; } = [];

        [JsonPropertyName("ThirdPartyAccount")]
        public List<object> ThirdPartyAccount { get; set; } = [];

        [JsonPropertyName("TicketOwner")]
        public List<object> TicketOwner { get; set; } = [];
    }
}