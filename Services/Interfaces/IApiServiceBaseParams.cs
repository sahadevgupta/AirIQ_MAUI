using System;

namespace AirIQ.Services.Interfaces;

public interface IApiServiceBaseParams
{
    IConnectivityService Connectivity { get; }
    ISecureStorageService SecureStorageService { get; }
}
