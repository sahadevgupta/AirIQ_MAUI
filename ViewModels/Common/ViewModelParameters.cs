using AirIQ.Services.Interfaces;

namespace AirIQ.ViewModels.Common
{
    public class ViewModelParameters : IViewModelParameters
    {
        public ViewModelParameters(ILoadingPopUpService loadingPopUpService,
            INavigationService navigationService,
            IShellNavigationService shellNavigationService)
        {
            LoadingPopUpService = loadingPopUpService;
            NavigationService = navigationService;
            ShellNavigationService = shellNavigationService;
        }
        public ILoadingPopUpService LoadingPopUpService { get; }
        public INavigationService NavigationService { get; }
        public IShellNavigationService ShellNavigationService { get; }
    }
}
