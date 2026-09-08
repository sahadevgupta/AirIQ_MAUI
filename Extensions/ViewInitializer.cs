using AirIQ.Views;

namespace AirIQ.Extensions
{
    public static class ViewInitializer
    {
        public static MauiAppBuilder ViewInit(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPage>()
                            .AddTransient<DashboardPage>()
                            .AddTransient<DashboardPage2>()
                            .AddTransient<DepartureDatePage>()
                            // Singleton: pre-warmed from DashboardPage2 so its calendar is already
                            // built by the time the user taps the departure/return date field - see
                            // TravelDatesPageViewModel.Preload().
                            .AddSingleton<TravelDatesPage>();

            return builder;
        }
    }
}
