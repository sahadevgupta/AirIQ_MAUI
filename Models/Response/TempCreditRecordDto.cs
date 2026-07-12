using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record TempCreditRecordDto
    {
        [JsonPropertyName("CreditID")]
        public int CreditID { get; set; }

        [JsonPropertyName("Date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("TempAmount")]
        public string? TempAmount { get; set; }

        [JsonPropertyName("Amount")]
        public double Amount { get; set; }
    }
}