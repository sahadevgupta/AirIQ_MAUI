using AirIQ.Constants;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels.Common;

public partial class ForgotPasswordPageViewModel(IViewModelParameters viewModelParameters,
    IAuthenticationService authenticationService) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]
    private string transactionKey = string.Empty;


    [ObservableProperty]
    private string? _username;

    [ObservableProperty]
    private string _otpValue = string.Empty;

    [ObservableProperty]
    private bool _isForgotVerificationViewVisible;
    #endregion

    #region [ Commnds ]

    [RelayCommand]
    private async Task Continue()
    {
        if (string.IsNullOrWhiteSpace(Username))
            return;

        try
        {
            using (LoadingService.Show())
            {
                transactionKey = await authenticationService.ForgotPasswordAsync(Username);
            }

            if (!string.IsNullOrWhiteSpace(transactionKey))
                IsForgotVerificationViewVisible = true;
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task Verify()
    {
        try
        {
            bool response;

            using (LoadingService.Show())
            {
                response = await authenticationService.VerifyOtpAsync(transactionKey, OtpValue);
            }

            if (response)
            {
                await ShellNavigationService.Navigate<ChangePasswordPage>(parameters: new Dictionary<string, object>
                {
                    { NavigationParamConstants.TransactionKey, transactionKey },
                    { NavigationParamConstants.Value, OtpValue },
                });
            }
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
    }

    #endregion

}