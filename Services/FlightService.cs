using System;
using System.Globalization;
using AirIQ.Configurations.CustomExceptions;
using AirIQ.Constants;
using AirIQ.Models.Request;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;

namespace AirIQ.Services;

public class FlightService(IApiServiceBaseParams apiServiceBaseParams, IApiCacheService apiCacheService) : ApiServiceBase(apiServiceBaseParams), IFlightService
{
    public async Task<IEnumerable<FlightRouteDto>> GetAvailableRoutesAsync()
    {
        IEnumerable<FlightRouteDto> flightRouteDtos = Enumerable.Empty<FlightRouteDto>();
        try
        {
            var cached = await apiCacheService.GetAsync<List<FlightRouteDto>>(
                ApiCacheKeys.FlightSectors,
                async () =>
                {
                    await Connectivity.CheckConnected();
                    var headers = await GetHeader();

                    var response = await BackendService.GetAvailableSectors(headers).ConfigureAwait(false);
                    var routes = SetResponse(Enumerable.Empty<FlightRouteDto>(), response);
                    return routes.ToList();
                });

            flightRouteDtos = cached ?? Enumerable.Empty<FlightRouteDto>();
        }
        catch (NotConnectedException notConntectedException)
        {
            HandleException(notConntectedException);
        }
        catch (UnauthorizedAccessException unauthorizedAccessException)
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

            if (response?.Data != null)
            {
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
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
        return availableDates;
    }

    public async Task<IEnumerable<FlightSearchResultDto>> GetFlightAvailabilityAsync(FlightSearchRequest request)
    {
        IEnumerable<FlightSearchResultDto>? flightAvailabilityResponse = Enumerable.Empty<FlightSearchResultDto>();
        try
        {
            var cacheKey = ApiCacheKeys.FlightSearch(request.Origin, request.Destination, request.DepartureDate,
                request.ReturnDate, request.Adult, request.Child, request.Infant, request.AirlineCode);

            var cached = await apiCacheService.GetAsync<List<FlightSearchResultDto>>(
                cacheKey,
                async () =>
                {
                    await Connectivity.CheckConnected();
                    var headers = await GetHeader();

                    var response = await BackendService.SearchFlights(request, headers).ConfigureAwait(false);
                    var results = SetResponse(Enumerable.Empty<FlightSearchResultDto>(), response);
                    return results.ToList();
                });

            flightAvailabilityResponse = cached ?? Enumerable.Empty<FlightSearchResultDto>();
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
        return flightAvailabilityResponse;
    }

    public async Task<string?> ConfirmBookingAsync(TicketBookingRequest ticketBookingRequest)
    {
        string? msg = string.Empty;
        try
        {
            await Connectivity.CheckConnected();
            var headers = await GetHeader();

            var response = await BackendService.TicketBooking(ticketBookingRequest, headers).ConfigureAwait(false);
            msg = response?.Message;
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
        return msg;
    }
}
