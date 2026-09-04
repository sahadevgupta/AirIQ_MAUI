using AirIQ.Configurations;
using AirIQ.Constants;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels;

public enum BiometricPageState
{
    Ready,
    Authenticating,
    Failed,
    Unavailable
}

[QueryProperty(nameof(IsPostLoginPrompt), NavigationParamConstants.IsPostLoginBiometricPrompt)]
public partial class BiometricAuthenticationPageViewModel : BaseViewModel
{
    private readonly IBiometricAuthenticationService _biometricService;

    [ObservableProperty]
    private BiometricPageState _state = BiometricPageState.Ready;

    [ObservableProperty]
    private string? _statusMessage;

    [ObservableProperty]
    private bool _isReady = true;

    [ObservableProperty]
    private bool _isAuthenticating;

    [ObservableProperty]
    private bool _isFailed;

    [ObservableProperty]
    private bool _isUnavailable;

    [ObservableProperty]
    private bool _isAvailable = true;

    // Set when this page is shown right after a successful login (offering to turn
    // biometric login on) rather than opened from MyAccountPage's Security row. In that
    // flow there's nothing to go "back" to (biometrics isn't set up yet), so both finishing
    // and skipping continue on to Home instead of navigating back.
    [ObservableProperty]
    private bool _isPostLoginPrompt;

    [ObservableProperty]
    private bool _canGoBack = true;

    public BiometricAuthenticationPageViewModel(IViewModelParameters viewModelParameters, IBiometricAuthenticationService biometricService) : base(viewModelParameters)
    {
        _biometricService = biometricService;

        // Keep the Authenticate button's enabled state (and re-entrancy guard) in sync with
        // IsBusy. IsBusy is declared on BaseViewModel, so it can't be hooked via a generated
        // OnIsBusyChanged partial method here - a PropertyChanged subscription is used instead.
        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(IsBusy))
                AuthenticateCommand.NotifyCanExecuteChanged();
        };
    }

    partial void OnStateChanged(BiometricPageState value)
    {
        IsReady = value == BiometricPageState.Ready;
        IsAuthenticating = value == BiometricPageState.Authenticating;
        IsFailed = value == BiometricPageState.Failed;
        IsUnavailable = value == BiometricPageState.Unavailable;
        IsAvailable = value != BiometricPageState.Unavailable;
    }

    partial void OnIsPostLoginPromptChanged(bool value) => CanGoBack = !value;

    private async Task ContinueAsync()
    {
        if (IsPostLoginPrompt)
            await MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync("//app/home"));
        else
            await ShellNavigationService.NavigateBack();
    }

    public override async Task LoadDataWhenNavigatedTo(CancellationToken cancellationToken = default)
    {
        await base.LoadDataWhenNavigatedTo(cancellationToken);

        var availability = await _biometricService.CheckAvailabilityAsync();
        if (availability != BiometricAvailability.Available)
        {
            StatusMessage = AppResource.BiometricNotAvailableMessage;
            State = BiometricPageState.Unavailable;
        }
    }

    private bool CanAuthenticate() => !IsBusy;

    [RelayCommand(CanExecute = nameof(CanAuthenticate))]
    private async Task Authenticate()
    {
        if (IsBusy)
            return;

        IsBusy = true;
        State = BiometricPageState.Authenticating;
        StatusMessage = null;

        try
        {
            var result = await _biometricService.AuthenticateAsync(AppResource.BiometricAuthDescription);

            switch (result.Status)
            {
                case BiometricAuthenticationStatus.Success:
                    AppConfiguration.IsBiometricLoginEnabled = true;
                    await ContinueAsync();
                    break;

                case BiometricAuthenticationStatus.Cancelled:
                    State = BiometricPageState.Ready;
                    break;

                case BiometricAuthenticationStatus.LockedOut:
                    StatusMessage = AppResource.BiometricAuthLockedOutMessage;
                    State = BiometricPageState.Failed;
                    break;

                case BiometricAuthenticationStatus.Unavailable:
                    StatusMessage = AppResource.BiometricNotAvailableMessage;
                    State = BiometricPageState.Unavailable;
                    break;

                default:
                    StatusMessage = AppResource.BiometricAuthFailedMessage;
                    State = BiometricPageState.Failed;
                    break;
            }
        }
        catch (Exception exception)
        {
            HandleException(exception, "Unhandled exception during biometric Authenticate()");
            StatusMessage = AppResource.BiometricAuthFailedMessage;
            State = BiometricPageState.Failed;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task UsePasscodeInstead()
    {
        await ContinueAsync();
    }
}
