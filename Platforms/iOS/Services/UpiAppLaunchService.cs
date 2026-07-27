using AirIQ.Services.Interfaces;
using AirIQ.Constants;
using Foundation;
using UIKit;

namespace AirIQ.Platforms.Services;

public class UpiAppLaunchService : IUpiAppLaunchService
{
    public async Task<UpiAppLaunchResult> LaunchAsync(string appKey)
    {
        try
        {
            var uriPrefixes = appKey switch
            {
                "GooglePay" => new[] { "tez://upi/pay", "gpay://upi/pay", "upi://pay" },
                "PhonePe" => new[] { "phonepe://upi/pay", "phonepe://pay", "upi://pay" },
                "Paytm" => new[] { "paytmmp://upi/pay", "paytmmp://pay", "upi://pay" },
                "BHIM" => new[] { "bhim://upi/pay", "upi://pay" },
                _ => Array.Empty<string>()
            };

            if (uriPrefixes.Length == 0)
                return new UpiAppLaunchResult(UpiAppLaunchStatus.InvalidApp, "Unsupported app key.");

            var query = UpiPaymentConstants.BuildPaymentQueryString();

            foreach (var prefix in uriPrefixes)
            {
                var url = new NSUrl($"{prefix}?{query}");
                if (url == null)
                    continue;

                // OpenUrlAsync returns whether iOS accepted opening the target app.
                var opened = await UIApplication.SharedApplication.OpenUrlAsync(url, new UIApplicationOpenUrlOptions());
                if (opened)
                    return new UpiAppLaunchResult(UpiAppLaunchStatus.Success);
            }

            return new UpiAppLaunchResult(UpiAppLaunchStatus.NotInstalled);
        }
        catch (Exception ex)
        {
            return new UpiAppLaunchResult(UpiAppLaunchStatus.LaunchFailed, ex.Message);
        }
    }

}
