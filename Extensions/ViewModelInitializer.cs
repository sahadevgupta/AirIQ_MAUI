using AirIQ.ViewModels;
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
                            .AddTransient<HotelsPageViewModel>();

            return builder;
        }
    }
}
