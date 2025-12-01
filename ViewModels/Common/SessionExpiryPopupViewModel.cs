using System;
using AirIQ.Constants;
using AirIQ.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Pages;
using Mopups.PreBaked.Interfaces;
using Mopups.Services;

namespace AirIQ.ViewModels.Common;

public partial class SessionExpiryPopupViewModel : ObservableObject
{
    readonly ILoginService _loginService;
    private TaskCompletionSource<bool> _sessionResponsTcs;

    [ObservableProperty]
    private string? _password;

    [ObservableProperty]
    private bool _isPasswordErrorVisible;

    public Task<bool> SessionResponseTask => _sessionResponsTcs.Task;

    public SessionExpiryPopupViewModel(ILoginService loginService)
    {
        _loginService = loginService;
        _sessionResponsTcs = new TaskCompletionSource<bool>();
    }

    #region [ Commands ]

    [RelayCommand]
    private async Task Confirm(PopupPage popup)
    {
        var userDto = await _loginService.LoginAsync(AppConfiguration.CurrentUser?.MobileNumber!, Password!);
        if(userDto != default)
        {
            _sessionResponsTcs.TrySetResult(true);
            await MopupService.Instance.PopAsync(true);
        }
    }

    #endregion

}
