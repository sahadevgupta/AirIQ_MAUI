using AirIQ.Controls;
using AirIQ.Extensions;
using AirIQ.Services;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels;
using AirIQ.ViewModels.Common;
using AirIQ.Views;
using CommunityToolkit.Maui;
using Mopups.Interfaces;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirIQ.Extensions
{
    public static class AppInitializer
    {
        public static MauiAppBuilder InitializeApp(this MauiAppBuilder builder)
        {
            builder
                .ConfigureAppFonts()
                .ConfigureAppHandlers()
                .ViewInit()
                .ViewModelInit()
                .RefitClientInit()
                .RegisterAppServices()
                .RegisterForNavigation();

            return builder;
        }


        private static MauiAppBuilder ConfigureAppFonts(this MauiAppBuilder builder)
        {
            return builder.ConfigureFonts(fonts =>
            {
                fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
                fonts.AddFont("Roboto-ExtraBold.ttf", "RobotoExtraBold");
                fonts.AddFont("Roboto-Italic.ttf", "RobotoItalic");
                fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
                fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                fonts.AddFont("Roboto-SemiBold.ttf", "RobotoSemiBold");
                fonts.AddFont("fa-solid-900.ttf", "MyFont");
            });
        }

        private static MauiAppBuilder ConfigureAppHandlers(this MauiAppBuilder builder)
        {
            return builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<CustomDropdown, AirIQ.Platforms.Handlers.CustomDropdownHandler>();
#if ANDROID
                handlers.AddHandler(typeof(Shell), typeof(AirIQ.Platforms.Handlers.CustomShellRenderer));
#endif
            });
        }
    }
}