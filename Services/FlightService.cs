using System;
using System.Globalization;
using AirIQ.Configurations.CustomExceptions;
using AirIQ.Models.Request;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;

namespace AirIQ.Services;

public class FlightService(IApiServiceBaseParams apiServiceBaseParams) : ApiServiceBase(apiServiceBaseParams), IFlightService
{
    public async Task<IEnumerable<FlightRouteDto>> GetAvailableRoutesAsync()
    {
        IEnumerable<FlightRouteDto> flightRouteDtos = Enumerable.Empty<FlightRouteDto>();
        try
        {
            await Connectivity.CheckConnected();
            var headers = await GetHeader();

            var response = await BackendService.GetAvailableSectors(headers).ConfigureAwait(false);
            flightRouteDtos = SetResponse(flightRouteDtos, response);
        }
        catch (NotConnectedException notConntectedException)
        {
            HandleException(notConntectedException);
        }
        catch(UnauthorizedAccessException unauthorizedAccessException)
        {
            HandleException(unauthorizedAccessException);
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
        return flightRouteDtos;
    }

    public async Task<IEnumerable<DateTime>> GetAvailableBookingDatesAsync(string origin, string destination)
    {
        List<DateTime> availableDates = new();
        try
        {
            await Connectivity.CheckConnected();
            var headers = await GetHeader();

            var request = new BookingDatesAvailabilityRequest
            {
                Origin = origin,
                Destination = destination
            };

            var response = await BackendService.DatesAvailability(request, headers).ConfigureAwait(false);

            foreach (var dateStr in response.Data)
            {
                if (DateTime.TryParseExact(dateStr, "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    availableDates.Add(date);
                }
                else
                {
                    Console.WriteLine($"Invalid date format: {dateStr}");
                }
            }
        }
        catch (Exception exception)
        {

        }
        return availableDates;
    }

    public async Task<IEnumerable<FlightSearchResultDto>> GetFlightAvailabilityAsync(FlightSearchRequest request)
    {
        IEnumerable<FlightSearchResultDto>? flightAvailabilityResponse = Enumerable.Empty<FlightSearchResultDto>();
        try
        {
            await Connectivity.CheckConnected();
            var headers = await GetHeader();

            var response = await BackendService.SearchFlights(request, headers).ConfigureAwait(false);
            flightAvailabilityResponse = SetResponse(flightAvailabilityResponse, response);
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
        return flightAvailabilityResponse;
    }
}
