using AirIQ.Constants;
using AirIQ.Models.Request;
using AirIQ.Models.Response;
using Refit;

namespace AirIQ.Services.Interfaces
{
    [Headers("Content-Type: application/json")]
    public interface IAppBackendService
    {
        // GET Calls
        [Get(UrlConstants.Sectors)]
        Task<ServiceResponse<IEnumerable<FlightRouteDto>>> GetAvailableSectors([HeaderCollection] IDictionary<string, string> headers);

        // POST CALLS
        [Post(UrlConstants.Availability)]
        Task<ServiceResponse<IEnumerable<string>>> DatesAvailability([Body(BodySerializationMethod.Serialized)] BookingDatesAvailabilityRequest request, [HeaderCollection] IDictionary<string, string> headers);

        [Post(UrlConstants.Login)]
        Task<LoginDto> Login([Body(BodySerializationMethod.Serialized)] LoginRequest request, [HeaderCollection] IDictionary<string, string> headers);

        [Post(UrlConstants.Search)]
        Task<ServiceResponse<IEnumerable<FlightSearchResultDto>>> SearchFlights([Body(BodySerializationMethod.Serialized)] FlightSearchRequest request, [HeaderCollection] IDictionary<string, string> headers);

        [Post(UrlConstants.Book)]
        Task<ServiceResponse<object>> TicketBooking([Body(BodySerializationMethod.Serialized)] TicketBookingRequest request, [HeaderCollection] IDictionary<string, string> headers);
    }
}
