namespace AirIQ.Extensions.Navigation
{
    public static class NavigationHelper
    {
        public static object GetPageModel(this IServiceProvider provider, Type T)
        {
            object context = null;
            try
            {
                context = provider.GetService(T);
            }
            catch (Exception exception)
            {
                throw new KeyNotFoundException($"No view with the name '{T.ToString()}' has been registered", exception);
            }
            return context;
        }
    }
}
