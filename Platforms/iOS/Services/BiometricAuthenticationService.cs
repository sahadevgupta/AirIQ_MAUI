using AirIQ.Services.Interfaces;

using LocalAuthentication;

namespace AirIQ.Platforms.Services;

public class BiometricAuthenticationService : IBiometricAuthenticationService
{
    public Task<BiometricAvailability> CheckAvailabilityAsync()
    {
        using var context = new LAContext();

        if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out var error))
            return Task.FromResult(BiometricAvailability.Available);

        return Task.FromResult(MapAvailability(error));
    }

    public Task<BiometricAuthenticationResult> AuthenticateAsync(string reason, CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<BiometricAuthenticationResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        var context = new LAContext();

        if (!context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out var error))
        {
            var availability = MapAvailability(error);
            var status = availability == BiometricAvailability.LockedOut
                ? BiometricAuthenticationStatus.LockedOut
                : BiometricAuthenticationStatus.Unavailable;

            tcs.SetResult(new BiometricAuthenticationResult(status, error?.LocalizedDescription));
            context.Dispose();
            return tcs.Task;
        }

        var registration = cancellationToken.Register(() => context.Invalidate());

        context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, reason, (success, evalError) =>
        {
            var result = success
                ? new BiometricAuthenticationResult(BiometricAuthenticationStatus.Success)
                : new BiometricAuthenticationResult(MapFailureStatus(evalError), evalError?.LocalizedDescription);

            tcs.TrySetResult(result);
        });

        _ = tcs.Task.ContinueWith(_ =>
        {
            registration.Dispose();
            context.Dispose();
        }, TaskScheduler.Default);

        return tcs.Task;
    }

    private static BiometricAvailability MapAvailability(global::Foundation.NSError? error)
    {
        if (error is null)
            return BiometricAvailability.Unknown;

        return (LAStatus)(long)error.Code switch
        {
            LAStatus.BiometryNotAvailable => BiometricAvailability.NoHardware,
            LAStatus.BiometryNotEnrolled => BiometricAvailability.NotEnrolled,
            LAStatus.BiometryLockout => BiometricAvailability.LockedOut,
            LAStatus.PasscodeNotSet => BiometricAvailability.HardwareUnavailable,
            _ => BiometricAvailability.Unknown
        };
    }

    private static BiometricAuthenticationStatus MapFailureStatus(global::Foundation.NSError? error)
    {
        if (error is null)
            return BiometricAuthenticationStatus.Failed;

        return (LAStatus)(long)error.Code switch
        {
            LAStatus.UserCancel or LAStatus.AppCancel or LAStatus.SystemCancel => BiometricAuthenticationStatus.Cancelled,
            LAStatus.UserFallback => BiometricAuthenticationStatus.Cancelled,
            LAStatus.BiometryLockout => BiometricAuthenticationStatus.LockedOut,
            LAStatus.BiometryNotAvailable or LAStatus.BiometryNotEnrolled or LAStatus.PasscodeNotSet => BiometricAuthenticationStatus.Unavailable,
            _ => BiometricAuthenticationStatus.Failed
        };
    }
}
