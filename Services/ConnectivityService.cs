using System;
using AirIQ.Configurations.CustomExceptions;
using AirIQ.Services.Interfaces;

namespace AirIQ.Services;

public class ConnectivityService : IConnectivityService
{
    public Task CheckConnected()
    {
        if (IsInternetAvailable())
        {
            return Task.CompletedTask;
        }

        throw new NotConnectedException("No Internet connectivity available");
    }


    private static bool IsInternetAvailable()
    {
        return Connectivity.NetworkAccess == NetworkAccess.Internet;
    }
}
