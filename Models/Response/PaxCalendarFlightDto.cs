using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record PaxCalendarFlightDto
    {
        [JsonPropertyName("TicketRefNo")]
        public string? TicketRefNo { get; set; }

        [JsonPropertyName("IsThirdParty")]
        public string? IsThirdParty { get; set; }

        [JsonPropertyName("Sector")]
        public string? Sector { get; set; }

        [JsonPropertyName("DepartureDate")]
        public DateTime DepartureDate { get; set; }

        [JsonPropertyName("DepTime")]
        public string? DepTime { get; set; }

        [JsonPropertyName("ArrTime")]
        public string? ArrTime { get; set; }

        [JsonPropertyName("AirlineCode")]
        public string? AirlineCode { get; set; }

        [JsonPropertyName("FlightNumber")]
        public string? FlightNumber { get; set; }

        [JsonPropertyName("PAX_Qty")]
        public int PAX_Qty { get; set; }

        [JsonPropertyName("InfantCount")]
        public int InfantCount { get; set; }

        [JsonPropertyName("PNR")]
        public string? PNR { get; set; }

        [JsonPropertyName("CheckinLink")]
        public string? CheckinLink { get; set; }

        [JsonPropertyName("StatusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("PassengerInformation")]
        public List<PassengerInformationDto> PassengerInformation { get; set; }
    }

    public record PaxCalendarResponseDto
    {
        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        [JsonPropertyName("Data")]
        public List<PaxCalendarFlightDto>? Data { get; set; }
    }
}