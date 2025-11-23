using System;
using AirIQ.Configurations.Mapper.Converters;
using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper;

public static class BackendToAppModelMapper
{
    public static User GetUser(UserDto? userDto)
    {
        if (userDto == null)
        {
            return new User();
        }
        var converter = new UserDtoToUseConverter();
        return converter.Convert(userDto);
    }

    public static IEnumerable<FlightRoute> GetAvailableRoutes(IEnumerable<FlightRouteDto> flightRouteDtos)
    {
        if (flightRouteDtos == null)
        {
            return new List<FlightRoute>();
        }
        var converter = new FlightRouteDtoToFlightRouteConverter();
        return flightRouteDtos.Select(converter.Convert);
    }

    public static IEnumerable<Flight> GetFlights(IEnumerable<FlightSearchResultDto> flightSearchResultDtos)
    {
        if (flightSearchResultDtos == null)
        {
            return new List<Flight>();
        }
        var converter = new FlightSearchResultDtoToFlight();
        return flightSearchResultDtos.Select(converter.Convert);
    }
}
