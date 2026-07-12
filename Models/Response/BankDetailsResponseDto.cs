using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record BankDetailsResponseDto
    {
        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        [JsonPropertyName("Data")]
        public Dictionary<string, CountryBankDetailsDto>? Data { get; set; }
    }
}