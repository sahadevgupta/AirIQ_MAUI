using AirIQ.Enums;
using AirIQ.Helpers;
using AirIQ.Popups;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views;

using CommunityToolkit.Mvvm.Input;

using Mopups.Interfaces;

namespace AirIQ.ViewModels
{
    public partial class SettingsViewModel(IViewModelParameters viewModelParameters, IAuthService authService) : BaseViewModel(viewModelParameters)
    {
        public string AppVersionDisplay => $"v{AppInfo.Current.VersionString}";

        #region [ Commands ]

        [RelayCommand]
        private void NotAvailable()
        {
            ShowToast(AppResource.FeatureComingSoon);
        }

        [RelayCommand]
        private async Task ManageAccount()
        {
            if (Shell.Current != null)
                await Shell.Current.GoToAsync("//app/AccountPage");
        }

        [RelayCommand]
        private async Task OpenTermsAndConditions()
        {
            await ShellNavigationService.Navigate<TermsAndConditionsPage>();
        }

        [RelayCommand]
        private async Task OpenPrivacyPolicy()
        {
            await ShellNavigationService.Navigate<PrivacyPolicyPage>();
        }

        [RelayCommand]
        private async Task AboutApp()
        {
            await ShowAlertAsync(string.Format(AppResource.AppNameVersionFormat, AppInfo.Current.VersionString), AlertType.Success);
        }

        [RelayCommand]
        private async Task ContactSupport()
        {
            var popup = new ContactUsPopup();
            await ServiceHelper.GetService<IPopupNavigation>()!.PushAsync(popup);
        }

        [RelayCommand]
        private async Task Logout()
        {
            var confirmed = await DialogService.DisplayAlertAsync(AppResource.AirIqDialogTitle, AppResource.LogoutConfirmationMessage, AppResource.OK, AppResource.Cancel);
            if (!confirmed) return;

            authService.Logout();

            if (Shell.Current != null)
                await MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync("//LoginPage"));
        }

        #endregion
    }
}
