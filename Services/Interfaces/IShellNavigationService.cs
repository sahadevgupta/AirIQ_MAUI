namespace AirIQ.Services.Interfaces
{
    public interface IShellNavigationService
    {
        Task Navigate<TPage>(bool isRootPage = false, IDictionary<string, object>? parameters = null) where TPage : Page;
        Task NavigateBack(int depth = 0, IDictionary<string, object>? parameters = null);
    }
}