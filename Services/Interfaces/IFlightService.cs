using System;
using AirIQ.Model.Request;
using AirIQ.Model.Response;

namespace AirIQ.Services.Interfaces;

public interface IFlightService
{
    Task<IEnumerable<FlightRouteDto>> GetAvailableRoutesAsync();
    Task<IEnumerable<DateTime>> GetAvailableBookingDatesAsync(string origin, string destination);

    Task<IEnumerable<FlightSearchResultDto>> GetFlightAvailabilityAsync(FlightSearchRequest request);
}
