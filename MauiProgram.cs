using AirIQ.Extensions;

using CommunityToolkit.Maui;

using Microsoft.Extensions.Logging;

using Mopups.Hosting;

using Syncfusion.Maui.Toolkit.Hosting;

using ZXing.Net.Maui.Controls;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using AirIQ.Services.Interfaces;
using Refit;
using SkiaSharp.Views.Maui.Controls.Hosting;

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
                .AddJsonConfiguration()
                .UseBarcodeReader()
                .ConfigureMopups()
                .InitializeApp()
                .ConfigureSyncfusionToolkit()
                .UseSkiaSharp()
                .UseSentry(options =>
                {
                    // The DSN is the only required setting.
                    options.Dsn = "https://862642371ff2f08eb430988eeeb163cf@o4510259879673856.ingest.de.sentry.io/4510259881508944";

                    // Use debug mode if you want to see what the SDK is doing.
                    // Debug messages are written to stdout with Console.Writeline,
                    // and are viewable in your IDE's debug console or with 'adb logcat', etc.
                    // This option is not recommended when deploying your application.
                    options.Debug = true;

                    // Other Sentry options can be set here.
                });

            IAppConfiguration configuration = builder.Services.BuildServiceProvider().GetRequiredService<IAppConfiguration>();
            builder.RefitClientInit(configuration);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static MauiAppBuilder AddJsonConfiguration(this MauiAppBuilder builder)
        {
            string appSettingFileName = $"appConfig.json";
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                                       .FirstOrDefault(name => name.EndsWith(appSettingFileName, StringComparison.OrdinalIgnoreCase));

            if (resourceName == null)
                throw new FileNotFoundException($"Embedded resource '{appSettingFileName}' not found");

            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                builder.Configuration.AddJsonStream(stream);
            }

            return builder;
        }

        private sealed class AppConfig
        {
            public ZoopConfig? Zoop { get; set; }
        }

        private sealed class ZoopConfig
        {
            public string? AppId { get; set; }
            public string? ApiKey { get; set; }
        }
    }
}
