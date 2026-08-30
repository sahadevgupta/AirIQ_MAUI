namespace AirIQ.Constants;

public static class UpiPaymentConstants
{
    public const string CallbackUrl = "airiq://upi-callback";
    public const string PayeeVpa = "MSAIRIQINDIALIMITED.eazypay@icici";
    public const string PayeeName = "M/S.AIRIQ 365 LIMITED FORMERLY KNOWN AS AIR IQ INDIA LIMITED";
    public const string MerchantCategoryCode = "4722";

    public static string BuildPaymentQueryString()
    {
        var callback = global::System.Uri.EscapeDataString(CallbackUrl);
        var payeeName = global::System.Uri.EscapeDataString(PayeeName);
        var payeeAddress = global::System.Uri.EscapeDataString(PayeeVpa);
        var transactionRef = global::System.Uri.EscapeDataString(GenerateTransactionRef());
        var merchantCategoryCode = global::System.Uri.EscapeDataString(MerchantCategoryCode);

        return $"upi://pay?pa={payeeAddress}&pn={payeeName}&tr={transactionRef}&cu=INR&mc={merchantCategoryCode}&url={callback}";
    }

    private static string GenerateTransactionRef()
    {
        var unixMillis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var randomSuffix = Random.Shared.Next(1000, 9999);
        return $"EZYS{unixMillis}{randomSuffix}";
    }
}
