using AirIQ.Enums;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AirIQ.Configurations
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            BaseUrl = configuration["ApiConfig:BaseUrl"] ?? throw new InvalidOperationException("Missing required configuration: ApiConfig:BaseUrl");
            ZoopApiKey = configuration["Zoop:ApiKey"] ?? throw new InvalidOperationException("Missing required configuration: Zoop:ApiKey");
            ZoopAppId = configuration["Zoop:AppId"] ?? throw new InvalidOperationException("Missing required configuration: Zoop:AppId");
        }

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

        public string BaseUrl { get; private set; }

        public string ApiKey { get; private set; }

        public string ZoopApiKey { get; private set; }

        public string ZoopAppId { get; private set; }

        public string ForgotPasswordPolicy { get; private set; }

        public string AuthorityBase { get; private set; }

        public string PortalUrl { get; private set; }
        public string PrivacyUrl { get; private set; }

        public string MicrosoftClarityProjectId { get; private set; }

        public static UserDto? CurrentUser;

        public static MenuType SelectedMenuType { get; set; } = MenuType.Flight;
    }
}
