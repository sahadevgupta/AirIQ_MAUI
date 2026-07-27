using AirIQ.Services.Interfaces;

namespace AirIQ.Services;

public class UpiPaymentCallbackService : IUpiPaymentCallbackService
{
    private readonly object _lock = new();
    private UpiPaymentCallbackData? _pending;

    public event EventHandler<UpiPaymentCallbackData>? CallbackReceived;

    public void HandleCallbackUri(string callbackUri)
    {
        if (string.IsNullOrWhiteSpace(callbackUri))
            return;

        if (!Uri.TryCreate(callbackUri, UriKind.Absolute, out var uri))
            return;

        if (!string.Equals(uri.Scheme, "airiq", StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(uri.Host, "upi-callback", StringComparison.OrdinalIgnoreCase))
            return;

        var query = ParseQuery(uri.Query);
        query.TryGetValue("status", out var rawStatus);
        query.TryGetValue("Status", out var rawStatusAlt);

        var effectiveRawStatus = string.IsNullOrWhiteSpace(rawStatus) ? rawStatusAlt ?? string.Empty : rawStatus;
        var status = ParseStatus(effectiveRawStatus);

        query.TryGetValue("txnId", out var txnId);
        query.TryGetValue("txnRef", out var txnRef);
        query.TryGetValue("responseCode", out var responseCode);
        query.TryGetValue("ApprovalRefNo", out var approvalRefNo);

        var callback = new UpiPaymentCallbackData(
            status,
            effectiveRawStatus,
            txnId,
            txnRef,
            responseCode,
            approvalRefNo,
            callbackUri);

        lock (_lock)
        {
            _pending = callback;
        }

        CallbackReceived?.Invoke(this, callback);
    }

    public bool TryConsumePending(out UpiPaymentCallbackData? callbackData)
    {
        lock (_lock)
        {
            callbackData = _pending;
            _pending = null;
            return callbackData != null;
        }
    }

    private static UpiPaymentStatus ParseStatus(string? rawStatus)
    {
        var normalized = rawStatus?.Trim().ToUpperInvariant();

        return normalized switch
        {
            "SUCCESS" => UpiPaymentStatus.Success,
            "S" => UpiPaymentStatus.Success,
            "FAILURE" => UpiPaymentStatus.Failure,
            "FAILED" => UpiPaymentStatus.Failure,
            "F" => UpiPaymentStatus.Failure,
            "SUBMITTED" => UpiPaymentStatus.Submitted,
            "PENDING" => UpiPaymentStatus.Submitted,
            "CANCELLED" => UpiPaymentStatus.Cancelled,
            "CANCELED" => UpiPaymentStatus.Cancelled,
            _ => UpiPaymentStatus.Unknown
        };
    }

    private static Dictionary<string, string> ParseQuery(string query)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(query))
            return result;

        var trimmed = query.TrimStart('?');
        if (string.IsNullOrWhiteSpace(trimmed))
            return result;

        var pairs = trimmed.Split('&', StringSplitOptions.RemoveEmptyEntries);
        foreach (var pair in pairs)
        {
            var split = pair.Split('=', 2);
            var key = Uri.UnescapeDataString(split[0]);
            var value = split.Length > 1 ? Uri.UnescapeDataString(split[1]) : string.Empty;
            result[key] = value;
        }

        return result;
    }
}
