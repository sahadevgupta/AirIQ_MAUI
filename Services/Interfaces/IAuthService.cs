using System;
using AirIQ.Models.Response;

namespace AirIQ.Services.Interfaces;

public interface IAuthService
{
    Task<AuthInfoDto> Auth();

    /// <summary>
    /// Clears the locally cached session (token, cached user, preferences) so that
    /// no protected functionality can be accessed until the user signs in again.
    /// </summary>
    void Logout();
}
