using AirIQ.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels.Common
{
    public abstract partial class BaseViewModel : ViewModelBase, IDestructible
    {
        protected readonly ILoadingPopUpService LoadingService;
        protected readonly INavigationService NavigationService;
        protected readonly IShellNavigationService ShellNavigationService;

        [ObservableProperty]
        private bool _isBusy;

        public BaseViewModel(IViewModelParameters parameters)
        {
            LoadingService = parameters.LoadingPopUpService;
            NavigationService = parameters.NavigationService;
            ShellNavigationService = parameters.ShellNavigationService;
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
