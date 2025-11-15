using AirIQ.Constants;
using AirIQ.Model.Request;
using AirIQ.Model.Response;
using Refit;

namespace AirIQ.Services.Interfaces
{
    [Headers("Content-Type: application/json")]
    public interface IAppBackendService
    {
        [Post(UrlConstants.Login)]
        Task<LoginDto> Login([Body(BodySerializationMethod.Serialized)] LoginRequest request, [HeaderCollection] IDictionary<string, string> headers);

    }
}
