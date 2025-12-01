using System;
using AirIQ.Models.Response;

namespace AirIQ.Services.Interfaces;

public interface IAuthService
{
    Task<AuthInfoDto> Auth();
}
