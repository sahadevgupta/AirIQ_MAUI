namespace AirIQ.Models
{
    public class City : LookupBase
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public int PromoCityId { get; set; }

        public List<object> ApiVendor { get; set; } = [];

        public List<object> ApiVendor1 { get; set; } = [];

        public List<object> FDestination { get; set; } = [];

        public List<object> Notification { get; set; } = [];

        public List<object> ThirdPartyAccount { get; set; } = [];

        public List<object> TicketOwner { get; set; } = [];
    }
}