using AirIQ.Constants;
using AirIQ.Services.Interfaces;
using AirIQ.Views;

namespace AirIQ
{
    public partial class App : Application
    {
        readonly INavigationService _navigationService;
        public App(INavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var navPage = new NavigationPage();
                MainPage = navPage;
                await _navigationService.Navigate(NavigationConstants.Login);
            });
        }
    }
}