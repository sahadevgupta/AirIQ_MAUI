using AirIQ.Extensions;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;

namespace AirIQ
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureMopups()
                .InitializeApp()
                .UseSentry(options => {
                   // The DSN is the only required setting.
                   options.Dsn = "https://862642371ff2f08eb430988eeeb163cf@o4510259879673856.ingest.de.sentry.io/4510259881508944";

                   // Use debug mode if you want to see what the SDK is doing.
                   // Debug messages are written to stdout with Console.Writeline,
                   // and are viewable in your IDE's debug console or with 'adb logcat', etc.
                   // This option is not recommended when deploying your application.
                   options.Debug = true;

                   // Other Sentry options can be set here.
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
