using AirIQ.Models.Response;

namespace AirIQ.Constants
{
    public static class AppConfiguration
    {
        public const string BaseUrl = "https://omairiq.azurewebsites.net";

        public const string ApiKey = "MTQ5NzcyNTA6UG9ydGFsIEFQSSBCb29raW5nOjE4NTc4MTczMTE5NjY6UGxDK1p5dE9wQ09QUG9XOEZvcXgrT25Fem5aSUF4WFo5ZGhuMDNSZ1FzST0=";
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

        public static UserDto? CurrentUser;
    }
}
