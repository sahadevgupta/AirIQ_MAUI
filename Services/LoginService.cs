using System.Text.Json;

using AirIQ.Configurations.CustomExceptions;
using AirIQ.Constants;
using AirIQ.Models.Request;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;

namespace AirIQ.Services;

public class LoginService(IApiServiceBaseParams apiServiceBaseParams,
    IAppBackendService appBackendService) : ApiServiceBase(apiServiceBaseParams), ILoginService
{
    public async Task<UserDto?> LoginAsync(string username, string password)
    {
        UserDto? userDto = new();
        try
        {
            Console.WriteLine("LoginAsync invoked : ");
            await Connectivity.CheckConnected();
            var request = new LoginRequest
            {
                UserName = username,
                Password = password,
            };
            Console.WriteLine("Login Without API invoked : ");
            var loginResponse = await appBackendService.LoginAsync(request).ConfigureAwait(false);
            Console.WriteLine("Login Without API response : " + JsonSerializer.Serialize(loginResponse));
            if (!string.IsNullOrWhiteSpace(loginResponse.Token))
            {
                await apiServiceBaseParams.SecureStorageService.SetAsync(PreferenceKeyConstants.AuthKey, loginResponse.Token!);
            }
            userDto = loginResponse.User;
        }
        catch (NotConnectedException notConntectedException)
        {
            HandleException(notConntectedException);
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
        return userDto;
    }
}
