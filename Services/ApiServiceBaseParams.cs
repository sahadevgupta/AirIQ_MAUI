using System;
using AirIQ.Services.Interfaces;

namespace AirIQ.Services;

public class ApiServiceBaseParams : IApiServiceBaseParams
{
    public IConnectivityService Connectivity { get; } 
    public ISecureStorageService SecureStorageService { get; }
    public ApiServiceBaseParams(
                IConnectivityService connectivityService,
                ISecureStorageService secureStorageService)
    {
        Connectivity = connectivityService;
        SecureStorageService = secureStorageService;
    }
}
