using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public class AccountLedgerRecordDto
    {
        [JsonPropertyName("RefNo")]
        public string? RefNo { get; set; }

        [JsonPropertyName("Date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("Particulars")]
        public string? Particulars { get; set; }

        [JsonPropertyName("Destination")]
        public string? Destination { get; set; }

        [JsonPropertyName("TravelDate")]
        public DateTime? TravelDate { get; set; }

        [JsonPropertyName("Amount")]
        public double Amount { get; set; }

        [JsonPropertyName("Balance")]
        public double Balance { get; set; }
    }
}