using AirIQ.Configurations.Mapper;
using AirIQ.Constants;
using AirIQ.Models;
using AirIQ.Services.Interfaces;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Font = Microsoft.Maui.Font;

namespace AirIQ.ViewModels.Common
{
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
            SentrySdk.CaptureException(exception);
#if DEBUG
            ShowToast(exception.Message);
#endif
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

        #region [ Commands ]

        [RelayCommand]
        private async Task Back()
        {
            await ShellNavigationService.NavigateBack();
        }

        #endregion
    }
}
