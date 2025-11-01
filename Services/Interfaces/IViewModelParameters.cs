namespace AirIQ.Services.Interfaces
{
    public interface IViewModelParameters
    {
        ILoadingPopUpService LoadingPopUpService { get; }
        INavigationService NavigationService { get; }
        IShellNavigationService ShellNavigationService { get; }
    }
}
