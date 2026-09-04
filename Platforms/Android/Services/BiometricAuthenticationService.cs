using AirIQ.Services.Interfaces;

using AndroidX.Biometric;
using AndroidX.Core.Content;

namespace AirIQ.Platforms.Services;

public class BiometricAuthenticationService : IBiometricAuthenticationService
{
    public Task<BiometricAvailability> CheckAvailabilityAsync()
    {
        var context = global::Android.App.Application.Context;
        var biometricManager = BiometricManager.From(context);
        var result = biometricManager.CanAuthenticate(BiometricManager.Authenticators.BiometricWeak);

        var availability = result switch
        {
            BiometricManager.BiometricSuccess => BiometricAvailability.Available,
            BiometricManager.BiometricErrorNoHardware => BiometricAvailability.NoHardware,
            BiometricManager.BiometricErrorHwUnavailable => BiometricAvailability.HardwareUnavailable,
            BiometricManager.BiometricErrorNoneEnrolled => BiometricAvailability.NotEnrolled,
            BiometricManager.BiometricErrorSecurityUpdateRequired => BiometricAvailability.HardwareUnavailable,
            _ => BiometricAvailability.Unknown
        };

        return Task.FromResult(availability);
    }

    public Task<BiometricAuthenticationResult> AuthenticateAsync(string reason, CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<BiometricAuthenticationResult>(TaskCreationOptions.RunContinuationsAsynchronously);

        if (Platform.CurrentActivity is not AndroidX.Fragment.App.FragmentActivity activity)
        {
            tcs.SetResult(new BiometricAuthenticationResult(BiometricAuthenticationStatus.Unavailable, "No current activity available."));
            return tcs.Task;
        }

        var executor = ContextCompat.GetMainExecutor(activity);
        var callback = new AuthenticationCallback(tcs);
        var biometricPrompt = new BiometricPrompt(activity, executor, callback);

        var promptInfo = new BiometricPrompt.PromptInfo.Builder()
            .SetTitle(reason)
            .SetNegativeButtonText("Cancel")
            .Build();

        var registration = cancellationToken.Register(() =>
        {
            try
            {
                biometricPrompt.CancelAuthentication();
            }
            catch (Java.Lang.Exception)
            {
                // Prompt may have already been dismissed - nothing further to do.
            }
        });

        _ = tcs.Task.ContinueWith(_ => registration.Dispose(), TaskScheduler.Default);

        biometricPrompt.Authenticate(promptInfo);

        return tcs.Task;
    }

    private sealed class AuthenticationCallback(TaskCompletionSource<BiometricAuthenticationResult> tcs) : BiometricPrompt.AuthenticationCallback
    {
        public override void OnAuthenticationSucceeded(BiometricPrompt.AuthenticationResult result)
        {
            base.OnAuthenticationSucceeded(result);
            tcs.TrySetResult(new BiometricAuthenticationResult(BiometricAuthenticationStatus.Success));
        }

        public override void OnAuthenticationError(int errorCode, Java.Lang.ICharSequence? errString)
        {
            base.OnAuthenticationError(errorCode, errString);

            var status = errorCode switch
            {
                BiometricPrompt.ErrorUserCanceled or BiometricPrompt.ErrorNegativeButton or BiometricPrompt.ErrorCanceled => BiometricAuthenticationStatus.Cancelled,
                BiometricPrompt.ErrorLockout or BiometricPrompt.ErrorLockoutPermanent => BiometricAuthenticationStatus.LockedOut,
                BiometricPrompt.ErrorNoBiometrics or BiometricPrompt.ErrorHwNotPresent or BiometricPrompt.ErrorHwUnavailable => BiometricAuthenticationStatus.Unavailable,
                _ => BiometricAuthenticationStatus.Failed
            };

            tcs.TrySetResult(new BiometricAuthenticationResult(status, errString?.ToString()));
        }

        public override void OnAuthenticationFailed()
        {
            base.OnAuthenticationFailed();
            // A single failed scan (e.g. wrong finger). The system prompt stays open for
            // retry, so the task must not complete here - only OnAuthenticationError/
            // OnAuthenticationSucceeded represent the prompt actually finishing.
        }
    }
}
