using AirIQ.Constants;
using AirIQ.Extensions.Navigation;
using AirIQ.ViewModels;
using AirIQ.ViewModels.Common;
using AirIQ.Views;

namespace AirIQ.Extensions
{
    public static class NavRegistryInitializer
    {
        public static MauiAppBuilder RegisterForNavigation(this MauiAppBuilder builder)
        {
            NavigationRegistry.Register<DashboardPage, DashboardPageViewModel>(NavigationConstants.Dashboard);
            NavigationRegistry.Register<HotelsPage, HotelsPageViewModel>(NavigationConstants.Hotels);
            NavigationRegistry.Register<LoginPage, LoginPageViewModel>(NavigationConstants.Login);

            return builder;
        }
    }
}
