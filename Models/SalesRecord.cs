namespace AirIQ.Models
{
    public class SalesRecord
    {
        public int SaleID { get; set; }
        public string? Prefix { get; set; }
        public bool Status { get; set; }
        public string? IsThirdParty { get; set; }
        public string? PNR { get; set; }
        public DateTime EntryDate { get; set; }
        public string? FDestName { get; set; }
        public DateTime TravelDateTime { get; set; }
        public string? AirlineName { get; set; }
        public int PAX_Qty { get; set; }
        public bool IsInternationalNew { get; set; }
        public double FinalRate { get; set; }
        public double Amount { get; set; }
        public List<SalesRecordPassenger>? Passengers { get; set; }
        public List<SalesRecordPassenger>? Infants { get; set; }
    }
}