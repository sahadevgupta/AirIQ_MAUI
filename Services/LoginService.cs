using System;
using AirIQ.Configurations.CustomExceptions;
using AirIQ.Constants;
using AirIQ.Model.Request;
using AirIQ.Model.Response;
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
            await Connectivity.CheckConnected();
            var request = new LoginRequest
            {
                UserName = username,
                Password = password,
            };
            var headers = await GetHeader();
            var loginResponse = await appBackendService.Login(request, headers).ConfigureAwait(false);
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
