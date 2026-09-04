namespace AirIQ.Services.Interfaces;

public interface IBiometricAuthenticationService
{
    Task<BiometricAvailability> CheckAvailabilityAsync();
    Task<BiometricAuthenticationResult> AuthenticateAsync(string reason, CancellationToken cancellationToken = default);
}

public enum BiometricAvailability
{
    Available,
    NoHardware,
    HardwareUnavailable,
    NotEnrolled,
    LockedOut,
    Unknown
}

public enum BiometricAuthenticationStatus
{
    Success,
    Cancelled,
    Failed,
    LockedOut,
    Unavailable
}

public sealed record BiometricAuthenticationResult(BiometricAuthenticationStatus Status, string? ErrorMessage = null);
