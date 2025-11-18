using System;
using AirIQ.Services.Interfaces;

namespace AirIQ.Services;

public class ApiServiceBaseParams : IApiServiceBaseParams
{
    public IAppBackendService BackendService { get; } 
    public IConnectivityService Connectivity { get; } 
    public ISecureStorageService SecureStorageService { get; }
    public ApiServiceBaseParams( IAppBackendService appBackendService,
                IConnectivityService connectivityService,
                ISecureStorageService secureStorageService)
    {
        BackendService = appBackendService;
        Connectivity = connectivityService;
        SecureStorageService = secureStorageService;
    }
}
