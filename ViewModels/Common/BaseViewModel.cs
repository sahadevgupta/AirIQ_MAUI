using AirIQ.Configurations.Mapper;
using AirIQ.Constants;
using AirIQ.Models;
using AirIQ.Services.Interfaces;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        public void ShowToast(string message, double toastfontSize = 14, ToastDuration toastDuration = ToastDuration.Short)
        {
            DialogService.ShowToast(message, toastfontSize, toastDuration);
        }

        public async Task ShowStatusAlertAsync(string message, bool response = true, int timeout = 2500)
        {
            await DialogService.ShowStatusAlertAsync(message, response, timeout);
        }

        public virtual Task LoadDataWhenNavigatedTo()
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
