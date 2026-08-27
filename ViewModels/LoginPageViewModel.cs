using System.Text.Json;
using AirIQ.Configurations;
using AirIQ.Constants;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Interfaces;
using Refit;

namespace AirIQ.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        readonly IAuthenticationService _loginService;
        readonly IPopupNavigation _popupNavigation;

        [ObservableProperty]
        private string? _username = "9380715388"; //string.Empty; //"9382915717";

        [ObservableProperty]
        private string? _password = "9380715388"; //"123456789";

        public LoginPageViewModel(IViewModelParameters viewModelParameters,
            IAuthenticationService loginService,
            IPopupNavigation popupNavigation) : base(viewModelParameters)
        {
            _loginService = loginService;
            _popupNavigation = popupNavigation;

            // Keep the Login button's enabled state (and re-entrancy guard) in sync with IsBusy.
            // IsBusy is declared on BaseViewModel, so it can't be hooked via a generated
            // OnIsBusyChanged partial method here - a PropertyChanged subscription is used instead.
            PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(IsBusy))
                    LoginCommand.NotifyCanExecuteChanged();
            };
        }

        #region [ Commands ]

        private bool CanLogin() => !IsBusy;

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task Login()
        {
            // Defense-in-depth: CanLogin/NotifyCanExecuteChanged already disables the button
            // while a login is in flight, but this keeps Login() safe even if it's ever invoked
            // directly (e.g. from code) instead of through the generated command.
            if (IsBusy)
            {
                Console.WriteLine("[Login] Ignored re-entrant Login() call while already busy.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await ShowAlertAsync("Please enter both username and password.");
                return;
            }

            IsBusy = true;
            try
            {
                Console.WriteLine("[Login] Login command started.");

                UserDto? userDto;
                LoadingService.Show();
                try
                {
                    userDto = await _loginService.LoginAsync(Username, Password);
                }
                finally
                {
                    // Awaited deliberately (not the `using`/Dispose pattern used elsewhere):
                    // the loader is a popup layered above the page, and we're about to navigate
                    // to Home right after this. If the popup's removal were left to fire-and-forget,
                    // it could still be mid-dismiss when Shell navigates, leaving it stuck on top
                    // of Home. HideAsync guarantees it's actually gone first.
                    await LoadingService.HideAsync();
                }

                Console.WriteLine($"[Login] LoginAsync returned user: {(userDto is null ? "null" : "present")}");

                if (userDto is not null)
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        AppConfiguration.IsLoggedInUser = true;
                        AppConfiguration.UserDetails = JsonSerializer.Serialize(userDto);
                        AppConfiguration.CurrentUser = userDto;

                        Console.WriteLine("[Login] Navigating to Home.");
                        await Shell.Current.GoToAsync("//app/home");
                    });
                }
                else
                {
                    Console.WriteLine("[Login] Login failed - no user returned, staying on Login page.");
                }
            }
            catch (ApiException apiEx)
            {
                Console.WriteLine("[Login] ApiException during login: " + apiEx);
                string message = "Login failed. Please try again.";
                try
                {
                    var content = string.IsNullOrWhiteSpace(apiEx.Content)
                        ? null
                        : JsonSerializer.Deserialize<ApiErrorResponse>(apiEx.Content);
                    if (!string.IsNullOrWhiteSpace(content?.Message))
                        message = content.Message;
                }
                catch (Exception parseEx)
                {
                    // apiEx.Content wasn't valid/expected JSON (e.g. an HTML error page from a
                    // gateway or timeout) - fall back to the generic message instead of letting
                    // this secondary exception escape the catch block and crash the app.
                    Console.WriteLine("[Login] Failed to parse ApiException content: " + parseEx);
                    SentrySdk.CaptureException(parseEx);
                }

                await ShowAlertAsync(message);
            }
            catch (Exception ex)
            {
                HandleException(ex, "Unhandled exception during Login()");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Signup()
        {
            await ShellNavigationService.Navigate<SignupPage>();
        }

        [RelayCommand]
        private async Task ForgotPassword()
        {
            await ShellNavigationService.Navigate<ForgotPasswordPage>();
        }

        [RelayCommand]
        private async Task DisplayContactUsViewAsync()
        {
            await _popupNavigation.PushAsync(new ContactUsPopup());
        }

        #endregion
    }
}
