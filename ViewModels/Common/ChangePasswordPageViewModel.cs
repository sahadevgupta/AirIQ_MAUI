using AirIQ.Services.Interfaces;
using AirIQ.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels.Common;

public partial class ChangePasswordPageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    [ObservableProperty]
    private bool _isPasswordReseted;
    #endregion

    #region [ Commands ]

    [RelayCommand]
    private async Task VerifyPassword()
    {
        IsPasswordReseted = true;

        await Task.Delay(2000);

        await ShellNavigationService.Navigate<LoginPage>(true);
    }

    #endregion

}