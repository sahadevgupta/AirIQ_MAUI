using AirIQ.Services;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Maui;

using Mopups.Interfaces;
using Mopups.Services;

namespace AirIQ.Extensions;

public static class AppServiceInitializer
{
    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
    {
        //Transient Services
        builder.Services.AddTransient<IViewModelParameters, ViewModelParameters>()
                        .AddTransient<IApiServiceBaseParams, ApiServiceBaseParams>()
                        .AddTransient<IDialogService, DialogService>()
                        .AddTransient<ILookupService, LookupService>()
                        .AddTransient<ILoginService, LoginService>()
                        .AddTransient<IFlightService, FlightService>()
                        .AddTransient<IAuthService, AuthService>()
                        .AddTransient<IOperationsService, OperationsService>();


        builder.Services.AddSingleton<ILoadingPopUpService, AirIQ.Platforms.Services.LoadingPopupService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IShellNavigationService, ShellNavigationService>()
                        .AddSingleton<IConnectivityService, ConnectivityService>()
                        .AddSingleton<ISecureStorageService, SecureStorageService>()
                        .AddSingleton<IPopupNavigation>(MopupService.Instance);


        return builder;
    }
}