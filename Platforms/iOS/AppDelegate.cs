using Foundation;
using AirIQ.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using UIKit;

namespace AirIQ
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (url?.Scheme?.Equals("airiq", StringComparison.OrdinalIgnoreCase) == true)
            {
                var callbackService = IPlatformApplication.Current?.Services.GetService<IUpiPaymentCallbackService>();
                callbackService?.HandleCallbackUri(url.AbsoluteString ?? string.Empty);
                return true;
            }

            return base.OpenUrl(app, url, options);
        }
    }
}
