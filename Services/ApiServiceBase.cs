using System;
using System.Diagnostics;
using AirIQ.Constants;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace AirIQ.Services;

public abstract class ApiServiceBase
{
    protected IConnectivityService Connectivity;
    protected IAppBackendService BackendService;
    readonly ISecureStorageService _secureStorageService;
    public ApiServiceBase(IApiServiceBaseParams parameters)
    {
        Connectivity = parameters.Connectivity;
        BackendService = parameters.BackendService;
        _secureStorageService = parameters.SecureStorageService;
    }

    protected void HandleException(Exception exception)
    {
        //Handle the exception
        Debug.WriteLine("ApiServiceBase HandleException [{exceptionName}] \n{exceptionToString}", exception.GetType().Name, exception.ToString());


        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        ToastDuration duration = ToastDuration.Short;
        double fontSize = 14;

        var toast = Toast.Make(exception.Message, duration, fontSize);

        toast.Show(cancellationTokenSource.Token);
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
    public IEnumerable<T> SetResponse<T>(IEnumerable<T> list, ServiceResponse<IEnumerable<T>> response)
    {
        if (response != null)
        {
            list = SetResponseData(list, response);
        }
        return list;
    }
    public T SetResponse<T>(T modelObject, ServiceResponse<T> response)
    {
        if (response != null)
        {
            modelObject = SetResponseData(modelObject, response);
        }
        return modelObject;
    }
    public bool SetSaveResponse(bool IsSaved, ServiceResponse<bool?> response)
    {
        if (response != null)
        {
            ErrorResponse(response);
            if (response.Data != null)
            {
                IsSaved = response.Data.Value;
            }
        }
        return IsSaved;
    }
    private T SetResponseData<T>(T modelObject, ServiceResponse<T> response)
    {
        if (response.Data != null)
        {
            modelObject = response.Data;
        }
        else
        {
            ErrorResponse(response);
        }

        return modelObject;
    }
    private IEnumerable<T> SetResponseData<T>(IEnumerable<T> list, ServiceResponse<IEnumerable<T>> response)
    {
        if (response.Data != null)
        {
            list = response.Data;
        }
        else
        {
            ErrorResponse(response);
        }

        return list;
    }

    public void ErrorResponse<T>(ServiceResponse<T> response)
    {
        // if (response.Error != null)
        // {
        //     Log.Error(CorrelationId + response.Error.ErrorCode.ToString(), response.Error.Message);
        //     SetException(response);
        //     return;
        // }
    }
}
