using System;
using AirIQ.Models.Request;
using AirIQ.Models.Response;

namespace AirIQ.Services.Interfaces;

public interface IFlightService
{
    Task<IEnumerable<FlightRouteDto>> GetAvailableRoutesAsync();
    Task<IEnumerable<DateTime>> GetAvailableBookingDatesAsync(string origin, string destination);

    Task<IEnumerable<FlightSearchResultDto>> GetFlightAvailabilityAsync(FlightSearchRequest request);
    Task ConfirmBookingAsync(TicketBookingRequest ticketBookingRequest);
}
