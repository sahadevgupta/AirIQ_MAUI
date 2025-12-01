using AirIQ.Constants;
using AirIQ.Controls;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;
using AirIQ.Views;
using Newtonsoft.Json;

namespace AirIQ
{
    public partial class App : Application
    {
        readonly INavigationService _navigationService;
        public App(INavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new NavigationPage(new LoadingIndicatorView()));

            window.Created += (s, e) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (AppConfiguration.IsLoggedInUser)
                    {
                        AppConfiguration.CurrentUser = JsonConvert.DeserializeObject<UserDto>(AppConfiguration.UserDetails);
                        window.Page = new AppShell();
                    }
                    else
                    {
                        await _navigationService.Navigate(NavigationConstants.Login);
                    }
                });
            };

            return window;
        }

    }
}