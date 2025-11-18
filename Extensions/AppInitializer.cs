using AirIQ.Controls;
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

        private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
        {
            //Transient Services
            builder.Services.AddTransient<IViewModelParameters, ViewModelParameters>()
                            .AddTransient<IApiServiceBaseParams, ApiServiceBaseParams>();


            builder.Services.AddSingleton<ILoadingPopUpService, AirIQ.Platforms.Services.LoadingPopupService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IShellNavigationService, ShellNavigationService>()
                            .AddSingleton<IConnectivityService, ConnectivityService>()
                            .AddSingleton<ISecureStorageService, SecureStorageService>()
                            .AddSingleton<ILoginService, LoginService>()
                            .AddSingleton<IFlightService,FlightService>()
                            .AddSingleton<IPopupNavigation>(MopupService.Instance);;


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