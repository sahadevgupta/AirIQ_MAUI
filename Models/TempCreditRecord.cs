namespace AirIQ.Models
{
    public class TempCreditRecord
    {
        public int CreditId { get; set; }

        public DateTime Date { get; set; }

        public string? Name { get; set; }

        public string? TempAmount { get; set; }

        public double Amount { get; set; }
    }
}