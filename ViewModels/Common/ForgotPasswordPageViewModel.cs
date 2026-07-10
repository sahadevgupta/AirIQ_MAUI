using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels.Common;

public partial class ForgotPasswordPageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    [ObservableProperty]
    private bool _isForgotVerificationViewVisible;
    #endregion

    #region [ Commnds ]

    [RelayCommand]
    private void Continue()
    {
        IsForgotVerificationViewVisible = true;
    }

    [RelayCommand]
    private async Task Verify()
    {
        await ShellNavigationService.Navigate<ChangePasswordPage>();
    }

    #endregion

}