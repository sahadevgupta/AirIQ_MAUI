using AirIQ.Configurations.Mapper;
using AirIQ.Constants;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace AirIQ.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        readonly ILoginService _loginService;

        [ObservableProperty]
        private string? _username = "9382915717";

        [ObservableProperty]
        private string? _password ="123456789";

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
                LoadingService.ShowLoading();
                IsBusy = true;

                var userDto = await _loginService.LoginAsync(Username!, Password!);

                LoadingService.HideLoading();

                if(userDto != default)
                {
                    AppConfiguration.IsLoggedInUser = true;
                    AppConfiguration.UserDetails = JsonConvert.SerializeObject(userDto);
                    
                    Application.Current!.Windows[0].Page = new AppShell();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., show an error message)
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
