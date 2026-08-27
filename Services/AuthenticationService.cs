using System.Text.Json;
using AirIQ.Configurations;
using AirIQ.Configurations.CustomExceptions;
using AirIQ.Constants;
using AirIQ.Models.Request;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;
using Refit;

namespace AirIQ.Services;


public class AuthenticationService(IApiServiceBaseParams apiServiceBaseParams,
    IAuthenticationApi appBackendService) : ApiServiceBase(apiServiceBaseParams), IAuthenticationService
{
    public async Task<UserDto?> LoginAsync(string username, string password)
    {
        // IMPORTANT: must start as null, not `new()`. LoginPageViewModel treats a non-null
        // return as a successful login and navigates to Home; a `new UserDto()` sentinel
        // left over from a caught exception below used to look like a real, empty user and
        // trigger a false-success navigation.
        UserDto? userDto = null;
        try
        {
            Console.WriteLine($"[Login] LoginAsync invoked for user '{username}'");
            await Connectivity.CheckConnected();
            var request = new LoginRequest
            {
                UserName = username,
                Password = password,
            };
            var loginResponse = await appBackendService.LoginAsync(request).ConfigureAwait(false);
            Console.WriteLine($"[Login] API responded, token present: {!string.IsNullOrWhiteSpace(loginResponse?.Token)}");
            if (loginResponse is not null && !string.IsNullOrWhiteSpace(loginResponse.Token))
            {
                await apiServiceBaseParams.SecureStorageService.SetAsync(PreferenceKeyConstants.AuthKey, loginResponse.Token!);
            }
            userDto = loginResponse?.User;
        }
        catch (ApiException)
        {
            throw;
        }
        catch (NotConnectedException notConntectedException)
        {
            Console.WriteLine("[Login] No connectivity: " + notConntectedException.Message);
            HandleException(notConntectedException);
        }
        catch (Exception exception)
        {
            Console.WriteLine("[Login] Unexpected error in LoginAsync: " + exception);
            HandleException(exception);
        }
        return userDto;
    }

    public async Task<string> ForgotPasswordAsync(string username)
    {
        string key = string.Empty;
        try
        {
            await Connectivity.CheckConnected();
            var request = new ForgotPasswordRequest
            {
                EmailOrPhone = username
            };
            var response = await appBackendService.ForgotPasswordAsync(request).ConfigureAwait(false);
            key = response.TransactionKey ?? string.Empty;
        }
        catch (NotConnectedException notConntectedException)
        {
            HandleException(notConntectedException);
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
        return key;
    }

    public async Task<bool> VerifyOtpAsync(string transactionKey, string otp)
    {
        bool isSuccess = false;
        try
        {
            Console.WriteLine("LoginAsync invoked : ");
            await Connectivity.CheckConnected();
            var request = new VerifyOtpRequest
            {
                TransactionKey = transactionKey,
                OTP = otp,
            };
            Console.WriteLine("Login Without API invoked : ");
            var response = await appBackendService.VerifyOtpAsync(request).ConfigureAwait(false);
            isSuccess = !string.IsNullOrWhiteSpace(response.Content) &&
                        response.Content.Contains("OTP verified successfully.", StringComparison.OrdinalIgnoreCase) ?
                            true : false;
        }
        catch (NotConnectedException notConntectedException)
        {
            HandleException(notConntectedException);
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
        return isSuccess;
    }

    public async Task<bool> ChangePasswordAsync(string oldPassword, string newPassword)
    {
        bool isSuccess = false;
        try
        {
            Console.WriteLine("LoginAsync invoked : ");
            await Connectivity.CheckConnected();
            var request = new ChangePasswordRequest
            {
                AccountId = AppConfiguration.CurrentUser?.AgencyId ?? 0,
                OldPassword = oldPassword,
                NewPassword = newPassword
            };
            Console.WriteLine("Login Without API invoked : ");
            var response = await appBackendService.ChangePasswordAsync(request).ConfigureAwait(false);
            isSuccess = !string.IsNullOrWhiteSpace(response.Content) &&
                        response.Content.Contains("Password changed successfully.", StringComparison.OrdinalIgnoreCase) ?
                            true : false;
        }
        catch (NotConnectedException notConntectedException)
        {
            HandleException(notConntectedException);
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
        return isSuccess;
    }

    public async Task<bool> ResetPasswordAsync(string transactionKey, string otp, string password)
    {
        bool isSuccess = false;
        try
        {
            Console.WriteLine("LoginAsync invoked : ");
            await Connectivity.CheckConnected();
            var request = new ResetPasswordRequest
            {
                TransactionKey = transactionKey,
                NewPassword = password,
                OTP = otp,
            };
            Console.WriteLine("Login Without API invoked : ");
            var response = await appBackendService.ResetPasswordAsync(request).ConfigureAwait(false);
            isSuccess = !string.IsNullOrWhiteSpace(response.Content) &&
                        response.Content.Contains("Password has been reset successfully.", StringComparison.OrdinalIgnoreCase) ?
                            true : false;
        }
        catch (NotConnectedException notConntectedException)
        {
            HandleException(notConntectedException);
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
        return isSuccess;
    }

    public async Task<string> SignupAsync(SignupRequest signupRequest)
    {
        string info = string.Empty;
        try
        {
            await Connectivity.CheckConnected();

            var apiResponse = await appBackendService.SignupAsync(signupRequest).ConfigureAwait(false);
            info = apiResponse.Message ?? string.Empty;
        }
        catch (ApiException apiEx)
        {
            info = apiEx.Content ?? string.Empty;
        }
        catch (NotConnectedException notConntectedException)
        {
            HandleException(notConntectedException);
        }
        catch (Exception exception)
        {
            HandleException(exception);

        }
        return info;
    }
}
