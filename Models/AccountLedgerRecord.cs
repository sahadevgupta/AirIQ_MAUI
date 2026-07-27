namespace AirIQ.Models
{
    public class AccountLedgerRecord
    {
        public string? RefNo { get; set; }

        public DateTime Date { get; set; }

        public string? Particulars { get; set; }

        public string? Destination { get; set; }

        public DateTime? TravelDate { get; set; }

        public double Amount { get; set; }

        public double Balance { get; set; }
    }
}