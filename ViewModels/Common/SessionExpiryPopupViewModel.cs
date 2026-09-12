using AirIQ.Configurations;
using AirIQ.Services.Interfaces;
using AirIQ.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Mopups.Services;

namespace AirIQ.ViewModels.Common;

public partial class SessionExpiryPopupViewModel : BaseViewModel
{
    readonly IAuthenticationService _loginService;
    readonly IAuthService _authService;
    private readonly TaskCompletionSource<bool> _sessionResponseTcs = new();

    [ObservableProperty]
    private string? _password;

    [ObservableProperty]
    private bool _isPasswordErrorVisible;

    public Task<bool> SessionResponseTask => _sessionResponseTcs.Task;

    public SessionExpiryPopupViewModel(IViewModelParameters viewModelParameters,
        IAuthService authService,
        IAuthenticationService loginService)
        : base(viewModelParameters)
    {
        _loginService = loginService;
        _authService = authService;
    }

    #region [ Commands ]

    // Sign In / Continue: hands off to the app's existing centralized Login page/flow
    // rather than re-implementing authentication inside this popup.
    [RelayCommand]
    private async Task SignIn()
    {
        await ClosePopupAsync();
        try
        {
            var userDto = await _loginService.LoginAsync(AppConfiguration.CurrentUser?.MobileNumber!, Password!);
            if (userDto != default)
            {
                _sessionResponseTcs.TrySetResult(true);
                return;
            }
        }
        catch (Exception exception)
        {
            // Must still resolve SessionResponseTask below even on failure - AuthService awaits
            // it as an app-wide gate before any API call, so an unhandled throw here would hang
            // every subsequent request in the app, not just this retry.
            Console.WriteLine("[SessionExpiry] SignIn failed: " + exception);
        }

        await ShellNavigationService.Navigate<LoginPage>(isRootPage: true);
        _sessionResponseTcs.TrySetResult(false);
    }

    [RelayCommand]
    private async Task ForgotPassword()
    {
        await ClosePopupAsync();
        await ShellNavigationService.Navigate<ForgotPasswordPage>();
        _sessionResponseTcs.TrySetResult(false);
    }

    // Sign in with a different account: clears the current session before returning
    // to Login so the next login doesn't inherit the previous user's cached state.
    [RelayCommand]
    private async Task SwitchAccount()
    {
        await ClosePopupAsync();
        _authService.Logout();
        await ShellNavigationService.Navigate<LoginPage>(isRootPage: true);
        _sessionResponseTcs.TrySetResult(false);
    }

    [RelayCommand]
    private async Task NotNow()
    {
        await ClosePopupAsync();
        _sessionResponseTcs.TrySetResult(false);
    }

    #endregion

    private static Task ClosePopupAsync() => MopupService.Instance.PopAsync(true);
}
