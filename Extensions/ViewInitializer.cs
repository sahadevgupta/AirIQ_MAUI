using AirIQ.Views;

namespace AirIQ.Extensions
{
    public static class ViewInitializer
    {
        public static MauiAppBuilder ViewInit(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPage>()
                            .AddTransient<DashboardPage>();

            return builder;
        }
    }
}
