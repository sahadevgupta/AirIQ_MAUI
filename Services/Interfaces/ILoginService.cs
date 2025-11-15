using System;
using AirIQ.Model.Response;

namespace AirIQ.Services.Interfaces;

public interface ILoginService
{
    Task<UserDto?> LoginAsync(string username, string password);
}
