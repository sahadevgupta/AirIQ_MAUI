using System;

namespace AirIQ.Services.Interfaces;

public interface IApiServiceBaseParams
{
    IAppBackendService BackendService { get; }
    IConnectivityService Connectivity { get; }
    ISecureStorageService SecureStorageService { get; }

}
