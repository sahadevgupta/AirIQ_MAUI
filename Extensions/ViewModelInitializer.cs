using AirIQ.ViewModels;

namespace AirIQ.Extensions
{
    public static class ViewModelInitializer
    {
        public static MauiAppBuilder ViewModelInit(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPageViewModel>();

            return builder;
        }
    }
}
