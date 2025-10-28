using AirIQ.Constants;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class LoginPageViewModel(ILoadingPopUpService loadingPopUpService,INavigationService navigationService) : BaseViewModel
    {
        #region [ Commands ]

        [RelayCommand]
        private async Task Login()
        {
            if (IsBusy)
                return;
            try
            {
                loadingPopUpService.ShowLoading();
                IsBusy = true;
                // Simulate a login process
                await Task.Delay(5000);

                loadingPopUpService.HideLoading();
                // Navigate to the dashboard or main page after successful login
                await navigationService.Navigate(NavigationConstants.Dashboard);
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
