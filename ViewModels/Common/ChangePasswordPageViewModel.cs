using System.Text.RegularExpressions;
using AirIQ.Constants;
using AirIQ.Services.Interfaces;
using AirIQ.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels.Common;

public partial class ChangePasswordPageViewModel(IViewModelParameters viewModelParameters,
    IAuthenticationService authenticationService) : BaseViewModel(viewModelParameters), IQueryAttributable
{
    #region [ Properties ]
    private string? transactionKey;
    private string? otp;


    [ObservableProperty]
    private bool _isPasswordReseted;

    [ObservableProperty]
    private string? _password;

    [ObservableProperty]
    private string? _confirmPassword;

    [ObservableProperty]
    private bool _isPasswordErrorVisible;

    [ObservableProperty]
    private bool _isConfirmPasswordErrorVisible;

    #endregion

    #region [ Methods & Service Calls ]

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey(NavigationParamConstants.TransactionKey))
        {
            transactionKey = query[NavigationParamConstants.TransactionKey]?.ToString();
            otp = query[NavigationParamConstants.Value]?.ToString();
        }
    }

    partial void OnPasswordChanged(string? value)
    {
        IsPasswordErrorVisible = string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, AppConstants.PasswordRegex);
        IsConfirmPasswordErrorVisible = string.IsNullOrWhiteSpace(ConfirmPassword)
                                     || !string.Equals(value, ConfirmPassword, StringComparison.Ordinal);
    }

    partial void OnConfirmPasswordChanged(string? value)
    {
        IsConfirmPasswordErrorVisible = string.IsNullOrWhiteSpace(value)
                                     || !string.Equals(Password, value, StringComparison.Ordinal);
    }

    #endregion

    #region [ Commands ]

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task VerifyPassword()
    {
        if (ValidateForm())
        {
            bool response = false;

            using (LoadingService.Show())
            {
                response = await authenticationService.ResetPasswordAsync(transactionKey!, otp!, Password!);
            }

            if (response)
            {
                IsPasswordReseted = true;

                await Task.Delay(2000);
                await ShellNavigationService.Navigate<LoginPage>(true);
            }
        }
    }

    private bool ValidateForm()
    {
        IsPasswordErrorVisible = string.IsNullOrWhiteSpace(Password)
                              || !Regex.IsMatch(Password, AppConstants.PasswordRegex);

        IsConfirmPasswordErrorVisible = string.IsNullOrWhiteSpace(ConfirmPassword)
                                     || !string.Equals(Password, ConfirmPassword, StringComparison.Ordinal);

        return !IsPasswordErrorVisible && !IsConfirmPasswordErrorVisible;
    }

    #endregion

}