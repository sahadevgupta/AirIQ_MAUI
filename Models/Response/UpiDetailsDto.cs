using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public class UpiDetailsDto
    {
        [JsonPropertyName("UpiId")]
        public string? UpiId { get; set; }

        [JsonPropertyName("QrImageUrl")]
        public string? QrImageUrl { get; set; }

        [JsonPropertyName("BgImageUrl")]
        public string? BgImageUrl { get; set; }

        [JsonPropertyName("Note")]
        public string? Note { get; set; }
    }
}