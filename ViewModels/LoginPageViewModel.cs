using AirIQ.Constants;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class LoginPageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
    {
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

                await Task.Delay(5000);

                LoadingService.HideLoading();

                Application.Current!.Windows[0].Page = new AppShell();
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
