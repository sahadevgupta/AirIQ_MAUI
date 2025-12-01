using System;
using AirIQ.Services.Interfaces;

namespace AirIQ.Services;

public class ApiServiceBaseParams : IApiServiceBaseParams
{
    public IAuthService AuthService {get;}
    public IAppBackendService BackendService { get; } 
    public IConnectivityService Connectivity { get; } 
    public ISecureStorageService SecureStorageService { get; }
    public ApiServiceBaseParams(IAuthService authService,
     IAppBackendService appBackendService,
                IConnectivityService connectivityService,
                ISecureStorageService secureStorageService)
    {
        AuthService =  authService;
        BackendService = appBackendService;
        Connectivity = connectivityService;
        SecureStorageService = secureStorageService;
    }
}
