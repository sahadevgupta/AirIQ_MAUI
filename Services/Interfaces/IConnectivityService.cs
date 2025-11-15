using System;

namespace AirIQ.Services.Interfaces;

public interface IConnectivityService
{
    Task CheckConnected();
}
