using AirIQ.Services.Interfaces;

namespace AirIQ.ViewModels.Common
{
    public class ViewModelParameters : IViewModelParameters
    {
        public ViewModelParameters(ILoadingPopUpService loadingPopUpService,
            IDialogService dialogService,
            INavigationService navigationService,
            IShellNavigationService shellNavigationService)
        {
            LoadingPopUpService = loadingPopUpService;
            DialogService = dialogService;
            NavigationService = navigationService;
            ShellNavigationService = shellNavigationService;
        }
        public ILoadingPopUpService LoadingPopUpService { get; }
        public IDialogService DialogService { get; }
        public INavigationService NavigationService { get; }
        public IShellNavigationService ShellNavigationService { get; }
    }
}
