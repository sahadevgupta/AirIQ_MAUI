using AirIQ.Services;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels;
using AirIQ.Views;
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
                .ViewInit()
                .ViewModelInit()
                .RefitClientInit()
                .RegisterAppServices()
                .RegisterForNavigation();

            return builder;
        }

        private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<ILoadingPopUpService, AirIQ.Platforms.Services.LoadingPopupService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();

            return builder;
        }
        private static MauiAppBuilder ConfigureAppFonts(this MauiAppBuilder builder)
        {
            return builder.ConfigureFonts(fonts =>
            {
                fonts.AddFont("Poppins-Bold.otf", "PoppinsBold");
                fonts.AddFont("Poppins-ExtraLight.otf", "PoppinsExtraLight");
                fonts.AddFont("Poppins-Medium.otf", "PoppinsMedium");
                fonts.AddFont("Poppins-Regular.otf", "PoppinsRegular");
                fonts.AddFont("Poppins-SemiBold.ttf", "PoppinsSemiBold");
                fonts.AddFont("fa-solid-900.ttf", "MyFont");
            });
        }
    }
}