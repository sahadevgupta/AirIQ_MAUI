using AirIQ.Constants;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Newtonsoft.Json;

namespace AirIQ.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        readonly ILoginService _loginService;

        [ObservableProperty]
        private string? _username = "9380715388"; //string.Empty; //"9382915717";

        [ObservableProperty]
        private string? _password = "9380715388"; //"123456789";

        public LoginPageViewModel(IViewModelParameters viewModelParameters,
        ILoginService loginService) : base(viewModelParameters)
        {
            _loginService = loginService;
        }

        #region [ Commands ]

        [RelayCommand]
        private async Task Login()
        {
            if (IsBusy)
                return;
            try
            {

                UserDto? userDto = null;
                using (LoadingService.Show())
                {
                    Console.WriteLine("Username : " + Username);
                    Console.WriteLine("Password : " + Password);
                    IsBusy = true;
                    userDto = await _loginService.LoginAsync(Username!, Password!);
                }

                Console.WriteLine("Login Detsils : " + JsonConvert.SerializeObject(userDto));
                if (userDto != default)
                {
                    AppConfiguration.IsLoggedInUser = true;
                    AppConfiguration.UserDetails = JsonConvert.SerializeObject(userDto);
                    AppConfiguration.CurrentUser = userDto;
                    Console.WriteLine("Navigate To Home : ");
                    await Shell.Current.GoToAsync("//app/home");
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
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

        #endregion
    }
}
