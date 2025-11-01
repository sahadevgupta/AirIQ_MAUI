using AirIQ.Services.Interfaces;

namespace AirIQ.Services
{
    public class ShellNavigationService : IShellNavigationService
    {
        public Task Navigate<TPage>(bool isRootPage = false, IDictionary<string, object>? parameters = null) where TPage : Page
        {
            var route = typeof(TPage).Name;
            route = isRootPage ? $"//{route}" : route;

            return parameters is null ?
                Shell.Current.GoToAsync(route) :
                Shell.Current.GoToAsync(route, parameters);
        }

        public Task NavigateBack(int depth = 0, IDictionary<string, object>? parameters = null)
        {
            string route = "..";
            for (int i = 0; i < depth; i++)
            {
                route += "/..";
            }
            return parameters is null ?
                Shell.Current.GoToAsync(route) :
                Shell.Current.GoToAsync(route, parameters);
        }
    }
}
