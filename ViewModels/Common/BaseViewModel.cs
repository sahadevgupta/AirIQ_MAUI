using AirIQ.Configurations;
using AirIQ.Configurations.Mapper;
using AirIQ.Constants;
using AirIQ.Extensions;
using AirIQ.Helpers;
using AirIQ.Models;
using AirIQ.Popups;
using AirIQ.Services.Interfaces;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Interfaces;
using Font = Microsoft.Maui.Font;
using NavigationMode = AirIQ.Enums.NavigationMode;

namespace AirIQ.ViewModels.Common;

public abstract partial class BaseViewModel : ViewModelBase, IDestructible
{
    protected readonly IDialogService DialogService;
    protected readonly ILoadingPopUpService LoadingService;
    protected readonly INavigationService NavigationService;
    protected readonly IShellNavigationService ShellNavigationService;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private User _currentUser = BackendToAppModelMapper.GetUser(AppConfiguration.CurrentUser);

    public BaseViewModel(IViewModelParameters parameters)
    {
        DialogService = parameters.DialogService;
        LoadingService = parameters.LoadingPopUpService;
        NavigationService = parameters.NavigationService;
        ShellNavigationService = parameters.ShellNavigationService;
    }

    protected void HandleException(Exception exception, string? message = null)
    {
        Console.WriteLine("ERROR Message : " + message);
        Console.WriteLine("Exception in VM " + exception);
        SentrySdk.CaptureException(exception);
        ShowToast(exception.Message);
    }

    public void ShowToast(string message, double toastfontSize = 14, ToastDuration toastDuration = ToastDuration.Short)
    {
        DialogService.ShowToast(message, toastfontSize, toastDuration);
    }

    public async Task ShowSnackBar(string message, bool isSuccess = true, double fontSize = 14, double duration = 3000, string? actionText = "", Action? action = null)
    {
        await DialogService.ShowSnackBarAync(message, isSuccess, fontSize, duration, actionText, action);

    }

    public async Task ShowStatusAlertAsync(string message, bool response = true, int timeout = 2500)
    {
        await DialogService.ShowStatusAlertAsync(message, response, timeout);
    }

    public virtual Task LoadDataWhenNavigatedTo()
    {
        return Task.CompletedTask;
    }
    public virtual Task LoadDataWhenOnAppearing()
    {
        return Task.CompletedTask;
    }
    public virtual Task LoadDataWhenOnDisappearing()
    {
        return Task.CompletedTask;
    }

    private async Task NavigateBackAsync()
    {
        await ShellNavigationService.NavigateBack();
    }

    #region [ Commands ]

    [RelayCommand]
    private async Task Navigate(NavigationMode navigationMode)
    {
        await (navigationMode switch
        {
            NavigationMode.Hamburger => OpenMenuAsync(),
            NavigationMode.Back => NavigateBackAsync(),
            _ => Task.CompletedTask
        });

    }

    [RelayCommand]
    private async Task OpenMenuAsync()
    {

        var popup = new MenuPopup();

        var popupservice = ServiceHelper.GetService<IPopupNavigation>();
        await popupservice?.PushAsync(popup)!;
    }

    #endregion
}

