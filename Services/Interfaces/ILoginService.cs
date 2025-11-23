using System;
using AirIQ.Models.Response;

namespace AirIQ.Services.Interfaces;

public interface ILoginService
{
    Task<UserDto?> LoginAsync(string username, string password);
}
