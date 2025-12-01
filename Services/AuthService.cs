using System;
using System.IdentityModel.Tokens.Jwt;
using AirIQ.Constants;
using AirIQ.Enums;
using AirIQ.Extensions;
using AirIQ.Models.Response;
using AirIQ.Popups;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using Mopups.Interfaces;

namespace AirIQ.Services;

public class AuthService(IConnectivityService connectivityService,
ISecureStorageService secureStorageService,
IPopupNavigation popupNavigation) : IAuthService
{
    public async Task<AuthInfoDto> Auth()
    {
        try
        {
            var _token = await secureStorageService.GetAsync(PreferenceKeyConstants.AuthKey);
            var _authInfo = ReadToken(_token);

            if (_authInfo == null)
            {
                throw new Exception();  //AuthServiceInteractiveLogonRequiredException("No cached credentials available");
            }

            if (!IsAuthInfoValid(_authInfo))
            {
                await connectivityService.CheckConnected();

                SessionExpiryPopupViewModel? vm = IPlatformApplication.Current?.Services.GetService<SessionExpiryPopupViewModel>();
                var sessionExpiryPopup = new SessionExpiryPopup
                {
                    BindingContext = vm
                };

               await MainThread.InvokeOnMainThreadAsync(async() =>
                {
                    await popupNavigation.PushAsync(sessionExpiryPopup);
                    bool result = await vm.SessionResponseTask;

                });


                //bool result = await vm.SessionResponseTask;

                var updatedToken = await secureStorageService.GetAsync(PreferenceKeyConstants.AuthKey);
                _authInfo = ReadToken(updatedToken);

            }

            return _authInfo;
        }
        catch (Exception exception)
        {
            return default;
        }
    }

    private bool IsAuthInfoValid(AuthInfoDto authInfo)
    {
        bool ret = true;

        if (authInfo == null)
        {
            Console.WriteLine($"IsAuthInfoValid [false] Authinfo is null");
            ret = false;
        }
        else if (authInfo.AccessTokenExpiresOn < DateTime.UtcNow)
        {
            Console.WriteLine($"IsAuthInfoValid [false] authInfo.AccessTokenExpiresOn[{authInfo.AccessTokenExpiresOn}] < DateTime.UtcNow[{DateTime.UtcNow}]", authInfo.AccessTokenExpiresOn, DateTime.UtcNow);

            ret = false;
        }

        Console.WriteLine($"IsAuthInfoValid [{ret}]");

        return ret;
    }

    public AuthInfoDto ReadToken(string accessToken)
    {
        try
        {
            var data = accessToken.Split(" ");
            var token = new JwtSecurityToken(data[1]);
            var authInfo = new AuthInfoDto
            {
                Status = AuthInfoStatusType.Valid
            };

            var exp = token.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            authInfo.AccessTokenExpiresOn = Convert.ToInt32(exp).ToDate().AddSeconds(-30);
            authInfo.AccessToken = accessToken;
            authInfo.UserName = token.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            authInfo.ApiKey = token.Claims.FirstOrDefault(c => c.Type == "apikey")?.Value;

            return authInfo;
        }
        catch (Exception)
        {
            return default(AuthInfoDto);
        }
    }
}
