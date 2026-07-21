using System.Text.Json.Serialization;

using AirIQ.Extensions;

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
        public List<PassengerInformationDto>? PassengerInformation { get; set; }

        [JsonIgnore]
        public string? SourceCity
        {
            get
            {
                var parts = Sector?.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts?.Length > 0)
                {
                    return parts[0].Trim();
                }
                return string.Empty;
            }
        }

        [JsonIgnore]
        public string? DestinationCity
        {
            get
            {
                var parts = Sector?.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts?.Length > 0)
                {
                    return parts[1].Trim();
                }
                return string.Empty;
            }
        }

        [JsonIgnore]
        public string SourceAirportCode
        {
            get
            {
                var code = SourceCity?.GetAirportCode() ?? string.Empty;
                return code;
            }
        }

        [JsonIgnore]
        public string DestinationAirportCode
        {
            get
            {
                var code = DestinationCity?.GetAirportCode() ?? string.Empty;
                return code;
            }
        }

    }

    public record PaxCalendarResponseDto
    {
        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        [JsonPropertyName("Data")]
        public List<PaxCalendarFlightDto>? Data { get; set; }
    }
}