using AirIQ.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirIQ.ViewModels.Common
{
    public partial class BaseViewModel : ViewModelBase, IDestructible
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
    }
}
