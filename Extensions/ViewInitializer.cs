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
                            .AddTransient<TravelDatesPage>();

            return builder;
        }
    }
}
