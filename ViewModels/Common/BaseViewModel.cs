using AirIQ.Configurations;
using AirIQ.Configurations.Mapper;
using AirIQ.Constants;
using AirIQ.Enums;
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
    private DateTime? _fromDate;

    [ObservableProperty]
    private DateTime? _toDate;

    [ObservableProperty]
    private User _currentUser = BackendToAppModelMapper.GetUser(AppConfiguration.CurrentUser);

    /// <summary>
    ///     True once a date range has been applied from the filter popup. Bind this to
    ///     <see cref="AirIQ.Controls.SearchView.IsFilterActive" /> to show the active-filter badge.
    /// </summary>
    public bool IsFilterActive => FromDate.HasValue || ToDate.HasValue;

    partial void OnFromDateChanged(DateTime? value) => OnPropertyChanged(nameof(IsFilterActive));

    partial void OnToDateChanged(DateTime? value) => OnPropertyChanged(nameof(IsFilterActive));

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

    public async Task ShowAlertAsync(string message, AlertType alertType = AlertType.Warning)
    {
        await DialogService.ShowAlertDialog(message, alertType);
    }

    public virtual Task LoadDataWhenNavigatedTo(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
    public virtual Task LoadDataWhenOnAppearing(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
    public virtual Task LoadDataWhenOnDisappearing(CancellationToken cancellationToken = default)
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

    [RelayCommand]
    async Task Filter()
    {
        var popup = new SearchFilterPopup { FromDate = FromDate, ToDate = ToDate };
        popup.Applied += async (from, to) =>
        {
            FromDate = from;
            ToDate = to;
            await OnDateFilterAppliedAsync(from, to);
        };
        await ServiceHelper.GetService<IPopupNavigation>()!.PushAsync(popup);
    }

    [RelayCommand]
    private async Task ClearFilter()
    {
        FromDate = null;
        ToDate = null;
        await OnDateFilterAppliedAsync(null, null);
    }

    /// <summary>
    ///     Called after the user applies (or clears) a date range from the filter popup, with the
    ///     values already assigned to <see cref="FromDate" />/<see cref="ToDate" />. Override in a
    ///     derived ViewModel to re-run its own query/filter using the selected range.
    /// </summary>
    protected virtual Task OnDateFilterAppliedAsync(DateTime? fromDate, DateTime? toDate) => Task.CompletedTask;

    #endregion
}

