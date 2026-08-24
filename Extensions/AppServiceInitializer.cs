using AirIQ.Configurations;
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
                        .AddTransient<IAuthenticationService, AuthenticationService>()
                        .AddTransient<IDialogService, DialogService>()
                        .AddTransient<IUpiAppLaunchService, AirIQ.Platforms.Services.UpiAppLaunchService>()
                        .AddTransient<IUpiPaymentCallbackService, UpiPaymentCallbackService>()
                        .AddTransient<ILookupService, LookupService>()
                        .AddTransient<IAuthenticationService, AuthenticationService>()
                        .AddTransient<IFlightService, FlightService>()
                        .AddTransient<IAuthService, AuthService>()
                        .AddTransient<IOperationsService, OperationsService>()
                        .AddTransient<IZoopVerificationService, ZoopVerificationService>();


        builder.Services.AddSingleton<ILoadingPopUpService, AirIQ.Platforms.Services.LoadingPopupService>()
                        .AddSingleton<INavigationService, NavigationService>()
                        .AddSingleton<IShellNavigationService, ShellNavigationService>()
                        .AddSingleton<IConnectivityService, ConnectivityService>()
                        .AddSingleton<ISecureStorageService, SecureStorageService>()
                        .AddSingleton<IPopupNavigation>(MopupService.Instance)
                        .AddSingleton<IAppConfiguration, AppConfiguration>();

        return builder;
    }
}