using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record GroupQueryResponseDto
    {
        [JsonPropertyName("GroupQueryID")]
        public int GroupQueryID { get; set; }

        [JsonPropertyName("TicketType")]
        public string? TicketType { get; set; }

        [JsonPropertyName("RouteDetails")]
        public string? RouteDetails { get; set; }

        [JsonPropertyName("TravelDates")]
        public string? TravelDates { get; set; }

        [JsonPropertyName("Pax")]
        public int Pax { get; set; }

        [JsonPropertyName("QuotedFare")]
        public double QuotedFare { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        [JsonPropertyName("RequestDate")]
        public string? RequestDate { get; set; }

        [JsonPropertyName("AdminNotes")]
        public string? AdminNotes { get; set; }
    }
}