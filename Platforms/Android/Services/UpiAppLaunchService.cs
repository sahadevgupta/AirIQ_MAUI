using AirIQ.Services.Interfaces;
using AirIQ.Constants;
using Android.Content;
using AndroidUri = global::Android.Net.Uri;

namespace AirIQ.Platforms.Services;

public class UpiAppLaunchService : IUpiAppLaunchService
{
    public Task<UpiAppLaunchResult> LaunchAsync(string appKey)
    {
        try
        {
            var context = global::Android.App.Application.Context;
            var packageManager = context.PackageManager;

            if (packageManager == null)
                return Task.FromResult(new UpiAppLaunchResult(UpiAppLaunchStatus.LaunchFailed, "Package manager unavailable."));

            var packageNames = appKey switch
            {
                "GooglePay" => new[] { "com.google.android.apps.nbu.paisa.user" },
                "PhonePe" => new[] { "com.phonepe.app" },
                "Paytm" => new[] { "net.one97.paytm" },
                "BHIM" => new[] { "in.org.npci.upiapp" },
                _ => Array.Empty<string>()
            };

            if (packageNames.Length == 0)
                return Task.FromResult(new UpiAppLaunchResult(UpiAppLaunchStatus.InvalidApp, "Unsupported app key."));

            var paymentUrl = BuildPaymentUrl();
            var paymentUri = AndroidUri.Parse(paymentUrl);

            foreach (var packageName in packageNames)
            {
                var launchIntent = new Intent(Intent.ActionView, paymentUri);
                launchIntent.SetPackage(packageName);

                if (launchIntent.ResolveActivity(packageManager) == null)
                    continue;

                launchIntent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(launchIntent);
                return Task.FromResult(new UpiAppLaunchResult(UpiAppLaunchStatus.Success));
            }

            var uriPrefixes = appKey switch
            {
                "GooglePay" => new[] { "tez://upi/pay", "gpay://upi/pay", "upi://pay" },
                "PhonePe" => new[] { "phonepe://upi/pay", "phonepe://pay", "upi://pay" },
                "Paytm" => new[] { "paytmmp://upi/pay", "paytmmp://pay", "upi://pay" },
                "BHIM" => new[] { "bhim://upi/pay", "upi://pay" },
                _ => Array.Empty<string>()
            };

            var queryString = UpiPaymentConstants.BuildPaymentQueryString();

            foreach (var prefix in uriPrefixes)
            {
                var intent = new Intent(Intent.ActionView, AndroidUri.Parse($"{prefix}?{queryString}"));
                intent.AddFlags(ActivityFlags.NewTask);

                if (intent.ResolveActivity(packageManager) == null)
                    continue;

                try
                {
                    context.StartActivity(intent);
                    return Task.FromResult(new UpiAppLaunchResult(UpiAppLaunchStatus.Success));
                }
                catch
                {
                    // Try next supported scheme.
                }
            }

            return Task.FromResult(new UpiAppLaunchResult(UpiAppLaunchStatus.NotInstalled));
        }
        catch (Exception ex)
        {
            return Task.FromResult(new UpiAppLaunchResult(UpiAppLaunchStatus.LaunchFailed, ex.Message));
        }
    }

    private static string BuildPaymentUrl() => $"upi://pay?{UpiPaymentConstants.BuildPaymentQueryString()}";
}
