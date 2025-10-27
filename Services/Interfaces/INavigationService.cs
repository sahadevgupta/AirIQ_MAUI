namespace AirIQ.Services.Interfaces
{
    public interface INavigationService
    {
        Task Navigate(string name, object parameter = null, bool animated = true, bool isModal = false, bool isMainview = false);
        Task NavigateBack(object parameter = null, bool animated = true, bool isModal = false);
        Task NavigateToRoot();
        Task NavigateBackToPage(string name, object parameter = null, bool animated = true, bool isModal = false);
    }
}
