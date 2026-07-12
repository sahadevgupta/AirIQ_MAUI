using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record SalesRecordDto
    {
        [JsonPropertyName("SaleID")]
        public int? SaleID { get; set; }

        [JsonPropertyName("Prefix")]
        public string? Prefix { get; set; }

        [JsonPropertyName("Status")]
        public bool? Status { get; set; }

        [JsonPropertyName("IsThirdParty")]
        public string? IsThirdParty { get; set; }

        [JsonPropertyName("PNR")]
        public string? PNR { get; set; }

        [JsonPropertyName("EntryDate")]
        public DateTime? EntryDate { get; set; }

        [JsonPropertyName("FDestName")]
        public string? FDestName { get; set; }

        [JsonPropertyName("TravelDateTime")]
        public DateTime? TravelDateTime { get; set; }

        [JsonPropertyName("aName")]
        public string? aName { get; set; }

        [JsonPropertyName("PAX_Qty")]
        public int? PAX_Qty { get; set; }

        [JsonPropertyName("IsInternationalNew")]
        public bool? IsInternationalNew { get; set; }

        [JsonPropertyName("FinalRate")]
        public double? FinalRate { get; set; }

        [JsonPropertyName("Amount")]
        public double? Amount { get; set; }

        [JsonPropertyName("Passengers")]
        public List<SalesRecordPassengerDto>? Passengers { get; set; }

        [JsonPropertyName("Infants")]
        public List<SalesRecordPassengerDto>? Infants { get; set; }
    }
}