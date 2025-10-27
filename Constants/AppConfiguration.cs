namespace AirIQ.Constants
{
    public static class AppConfiguration
    {
        public const string BaseUrl = "https://arbiitourism.tripdisk.com/api/";

        public static bool IsLoggedInUser
        {
            get => Preferences.Get(nameof(IsLoggedInUser), false);
            set => Preferences.Set(nameof(IsLoggedInUser), value);
        }

        public static string UserDetails
        {
            get => Preferences.Get(nameof(UserDetails), string.Empty);
            set => Preferences.Set(nameof(UserDetails), value);
        }
    }
}
