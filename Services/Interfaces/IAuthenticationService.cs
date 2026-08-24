using System;
using AirIQ.Models.Request;
using AirIQ.Models.Response;

namespace AirIQ.Services.Interfaces;

public interface IAuthenticationService
{
    Task<bool> ChangePasswordAsync(string oldPassword, string newPassword);
    Task<string> ForgotPasswordAsync(string username);
    Task<UserDto?> LoginAsync(string username, string password);
    Task<bool> ResetPasswordAsync(string transactionKey, string otp, string password);
    Task<string> SignupAsync(SignupRequest signupRequest);
    Task<bool> VerifyOtpAsync(string transactionKey, string otp);
}
