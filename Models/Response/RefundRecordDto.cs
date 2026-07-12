using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record RefundRecordDto
    {
        [JsonPropertyName("ReturnID")]
        public int ReturnID { get; set; }

        [JsonPropertyName("ThirdReturnID")]
        public int ThirdReturnID { get; set; }

        [JsonPropertyName("IsThirdParty")]
        public string? IsThirdParty { get; set; }

        [JsonPropertyName("Prefix")]
        public string? Prefix { get; set; }

        [JsonPropertyName("PNR")]
        public string? PNR { get; set; }

        [JsonPropertyName("EntryDate")]
        public DateTime EntryDate { get; set; }

        [JsonPropertyName("FDestName")]
        public string? FDestName { get; set; }

        [JsonPropertyName("TravelDateTime")]
        public DateTime TravelDateTime { get; set; }

        [JsonPropertyName("Qty")]
        public int Qty { get; set; }

        [JsonPropertyName("Amount")]
        public double Amount { get; set; }

        [JsonPropertyName("CancelChrg")]
        public double CancelChrg { get; set; }

        [JsonPropertyName("RefundAmount")]
        public double RefundAmount { get; set; }
    }
}