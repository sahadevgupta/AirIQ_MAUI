namespace AirIQ.Constants;

public static class UrlConstants
{
    public const string Availability = $"/{nameof(Availability)}";
    public const string Book = $"/{nameof(Book)}";
    public const string Login = $"/{nameof(Login)}";
    public const string LoginWithoutApiKey = $"/{nameof(LoginWithoutApiKey)}";
    public const string Sectors = $"/{nameof(Sectors)}";
    public const string Search = $"/{nameof(Search)}";
    public const string Countries = $"/api/listings/{nameof(Countries)}";
    public const string States = $"/api/listings/{nameof(States)}";
    public const string Cities = $"/api/listings/{nameof(Cities)}";
    public const string MainCities = $"/api/listings/{nameof(MainCities)}";
    public const string Districts = $"/api/listings/{nameof(Districts)}";
    public const string Accountmanagers = $"/api/listings/{nameof(Accountmanagers)}";
    public const string SalesRecords = "/api/sales/records";
    public const string RefundRecords = "/api/refunds/records";
    public const string AccountLedgerRecords = "/api/ledger/records";
    public const string TemporaryCreditRecords = "/api/temp-credit/records";
    public const string BankDetailsRecords = "/api/bank-details";
    public const string GroupQueryRecords = "/api/group-queries/records";
    public const string UploadRequest = "/api/upload-request/submit";
}
