using AirIQ.Configurations;
using AirIQ.Enums;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class MyAccountPageViewModel(IViewModelParameters viewModelParameters, IAuthService authService) : BaseViewModel(viewModelParameters)
    {
        [ObservableProperty]
        private string _biometricStatusText = string.Empty;

        [ObservableProperty]
        private Color _biometricStatusColor = Colors.Gray;

        public string AppVersionDisplay => $"v{AppInfo.Current.VersionString}";

        public string WalletBalanceDisplay => CurrentUser.Balance.HasValue ? $"₹ {CurrentUser.Balance:N2}" : "--";

        public override Task LoadDataWhenOnAppearing(CancellationToken cancellationToken = default)
        {
            RefreshBiometricStatus();
            return base.LoadDataWhenOnAppearing(cancellationToken);
        }

        private void RefreshBiometricStatus()
        {
            var isEnabled = AppConfiguration.IsBiometricLoginEnabled;
            BiometricStatusText = isEnabled ? AppResource.Enabled : AppResource.Disabled;
            BiometricStatusColor = (Color)Application.Current?.Resources[isEnabled ? "Green" : "Gray50"]!;
        }

        #region [ Commands ]

        [RelayCommand]
        private void NotAvailable()
        {
            ShowToast(AppResource.FeatureComingSoon);
        }

        [RelayCommand]
        private async Task OpenBiometricAuthentication()
        {
            await ShellNavigationService.Navigate<BiometricAuthenticationPage>();
        }

        [RelayCommand]
        private async Task ChangePassword()
        {
            await ShellNavigationService.Navigate<ForgotPasswordPage>();
        }

        [RelayCommand]
        private async Task OpenWallet()
        {
            await ShellNavigationService.Navigate<WalletPage>();
        }

        [RelayCommand]
        private async Task OpenAccountLedger()
        {
            await ShellNavigationService.Navigate<AccountLedgerRecordPage>();
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
        private async Task Logout()
        {
            authService.Logout();

            if (Shell.Current != null)
                await MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync("//LoginPage"));
        }

        [RelayCommand]
        private async Task RequestAccountDeletionByEmail()
        {
            await Launcher.Default.OpenAsync(new Uri($"mailto:{AppResource.SupportEmailAddress}"));
        }

        [RelayCommand]
        private async Task RequestAccountDeletionByPhone()
        {
            await Launcher.Default.OpenAsync(new Uri($"tel:{AppResource.SupportPhoneNumber.Replace(" ", "")}"));
        }

        #endregion
    }
}
