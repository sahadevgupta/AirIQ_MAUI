using AirIQ.Services;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using Mopups.Interfaces;
using Mopups.Services;

namespace AirIQ.Extensions;

public static class AppServiceInitializer
{
    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
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
                        .AddSingleton<IFlightService, FlightService>()
                        .AddSingleton<IPopupNavigation>(MopupService.Instance)
                        .AddSingleton<IAuthService, AuthService>();


        return builder;
    }
}