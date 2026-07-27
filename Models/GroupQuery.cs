namespace AirIQ.Models
{
    public class GroupQuery
    {
        public int GroupQueryId { get; set; }

        public string? TicketType { get; set; }

        public string? RouteDetails { get; set; }

        public string? TravelDates { get; set; }

        public int Pax { get; set; }

        public double QuotedFare { get; set; }

        public string? Status { get; set; }

        public string? RequestDate { get; set; }

        public string? AdminNotes { get; set; }
    }
}