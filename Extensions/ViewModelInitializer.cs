using AirIQ.ViewModels;
using AirIQ.ViewModels.Common;
using CommunityToolkit.Maui;

namespace AirIQ.Extensions
{
    public static class ViewModelInitializer
    {
        public static MauiAppBuilder ViewModelInit(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<ChangePasswordPageViewModel>()
                            .AddTransient<DashboardPageViewModel>()
                            .AddTransient<FlightsPageViewModel>()
                            .AddTransient<FlightBookingPageViewModel>()
                            .AddTransient<ForgotPasswordPageViewModel>()
                            .AddTransient<LoginPageViewModel>()
                            .AddTransient<MenuPageViewModel>()
                            .AddTransient<HotelsPageViewModel>()
                            .AddTransient<SalesRecordPageViewModel>()
                            .AddTransient<SignupPageViewModel>()
                            .AddTransient<SessionExpiryPopupViewModel>();

            return builder;
        }
    }
}
