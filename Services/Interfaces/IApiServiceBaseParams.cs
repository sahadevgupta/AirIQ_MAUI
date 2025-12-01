using System;

namespace AirIQ.Services.Interfaces;

public interface IApiServiceBaseParams
{
    IAuthService AuthService {get;}
    IAppBackendService BackendService { get; }
    IConnectivityService Connectivity { get; }
    ISecureStorageService SecureStorageService { get; }

}
