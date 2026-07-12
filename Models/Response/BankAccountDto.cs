using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record BankAccountDto
    {
        [JsonPropertyName("Title")]
        public string? Title { get; set; }

        [JsonPropertyName("AccountName")]
        public string? AccountName { get; set; }

        [JsonPropertyName("AccountNo")]
        public string? AccountNo { get; set; }

        [JsonPropertyName("AccountType")]
        public string? AccountType { get; set; }

        [JsonPropertyName("BankName")]
        public string? BankName { get; set; }

        [JsonPropertyName("IfscCode")]
        public string? IfscCode { get; set; }

        [JsonPropertyName("Branch")]
        public string? Branch { get; set; }

        [JsonPropertyName("Note")]
        public string? Note { get; set; }

        [JsonPropertyName("Warning")]
        public string? Warning { get; set; }

        [JsonPropertyName("ImageUrl")]
        public string? ImageUrl { get; set; }
    }
}