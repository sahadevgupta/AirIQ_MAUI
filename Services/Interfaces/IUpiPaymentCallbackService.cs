namespace AirIQ.Services.Interfaces;

public interface IUpiPaymentCallbackService
{
    event EventHandler<UpiPaymentCallbackData>? CallbackReceived;
    void HandleCallbackUri(string callbackUri);
    bool TryConsumePending(out UpiPaymentCallbackData? callbackData);
}

public enum UpiPaymentStatus
{
    Unknown,
    Success,
    Failure,
    Submitted,
    Cancelled
}

public sealed record UpiPaymentCallbackData(
    UpiPaymentStatus Status,
    string RawStatus,
    string? TransactionId,
    string? ReferenceId,
    string? ResponseCode,
    string? ApprovalRefNo,
    string RawUri);
