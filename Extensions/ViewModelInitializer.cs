using AirIQ.ViewModels;
using AirIQ.ViewModels.Common;
using CommunityToolkit.Maui;

namespace AirIQ.Extensions
{
    public static class ViewModelInitializer
    {
        public static MauiAppBuilder ViewModelInit(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPageViewModel>()
                            .AddTransient<DashboardPageViewModel>()
                            .AddTransient<FlightsPageViewModel>()
                            .AddTransient<FlightBookingPageViewModel>()
                            .AddTransient<HotelsPageViewModel>()
                            .AddTransient<SessionExpiryPopupViewModel>();

            return builder;
        }
    }
}
