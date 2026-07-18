using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record PassengerInformationDto
    {
        [JsonPropertyName("PassengerName")]
        public string? PassengerName { get; set; }

        [JsonPropertyName("InfantDetails")]
        public string? InfantDetails { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }
    }
}