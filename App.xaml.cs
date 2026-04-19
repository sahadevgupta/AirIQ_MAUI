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
        readonly IShellNavigationService _navigationService;
        public App(IShellNavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            window.Created += (s, e) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        if (AppConfiguration.IsLoggedInUser)
                        {
                            AppConfiguration.CurrentUser = JsonConvert.DeserializeObject<UserDto>(AppConfiguration.UserDetails);
                            await Shell.Current.GoToAsync("//app/home");
                        }
                        else
                        {
                            await _navigationService.Navigate<LoginPage>(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        SentrySdk.CaptureException(ex);
                    }
                });
            };

            return window;
        }

    }
}