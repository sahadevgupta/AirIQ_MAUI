namespace AirIQ.Services.Interfaces
{
    public interface IViewModelParameters
    {
        IDialogService DialogService { get; }
        ILoadingPopUpService LoadingPopUpService { get; }
        INavigationService NavigationService { get; }
        IShellNavigationService ShellNavigationService { get; }
    }
}
