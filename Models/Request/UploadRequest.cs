namespace AirIQ.Models.Request
{
    public class UploadRequest
    {
        public string? RefNumber { get; set; }
        public string? PaymentMode { get; set; }
        public string? FilePath { get; set; }
        public string? Message { get; set; }
        public decimal Amount { get; set; }
    }
}