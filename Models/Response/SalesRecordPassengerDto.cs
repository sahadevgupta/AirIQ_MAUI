using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record SalesRecordPassengerDto
    {
        [JsonPropertyName("PassengerName")]
        public string? PassengerName { get; set; }

        [JsonPropertyName("InfantCharges")]
        public double? InfantCharges { get; set; }
    }
}