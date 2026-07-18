namespace AirIQ.Models
{
    public class RefundRecord
    {
        public int ReturnId { get; set; }

        public int ThirdReturnId { get; set; }

        public string? IsThirdParty { get; set; }

        public string? Prefix { get; set; }

        public string? PNR { get; set; }

        public DateTime EntryDate { get; set; }

        public string? FDestName { get; set; }

        public DateTime TravelDateTime { get; set; }

        public int Qty { get; set; }

        public double Amount { get; set; }

        public double CancelChrg { get; set; }

        public double RefundAmount { get; set; }
    }
}