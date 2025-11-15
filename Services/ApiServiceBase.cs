using System;
using System.Diagnostics;
using AirIQ.Constants;
using AirIQ.Services.Interfaces;

namespace AirIQ.Services;

public abstract class ApiServiceBase
{
    protected IConnectivityService Connectivity;
    readonly ISecureStorageService _secureStorageService;
    public ApiServiceBase(IApiServiceBaseParams parameters)
    {
        Connectivity = parameters.Connectivity;
        _secureStorageService = parameters.SecureStorageService;
    }

    protected void HandleException(Exception exception)
    {
        //Handle the exception
        Debug.WriteLine("ApiServiceBase HandleException [{exceptionName}] \n{exceptionToString}", exception.GetType().Name, exception.ToString());
    }

    protected async Task<Dictionary<string, string>> GetHeader()
    {

        var header = new Dictionary<string, string> { };
        try
        {

            var authInfo = await _secureStorageService.GetAsync(PreferenceKeyConstants.AuthKey);
            if (!string.IsNullOrWhiteSpace(authInfo))
            {
                header.Add("Authorization", authInfo);
            }

            header.Add("api-key", AppConfiguration.ApiKey);
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
        return header;
    }
}
