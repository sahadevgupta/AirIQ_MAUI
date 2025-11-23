using System;
using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters;

public class FlightRouteDtoToFlightRouteConverter : ConverterBase<FlightRouteDto, FlightRoute>
{
    protected override FlightRoute ConvertImpl(FlightRouteDto source)
    {
        return new FlightRoute
        {
           Destination = source.Destination,
           Origin = source.Origin,
           Sector = source.Sector
        };
    }
}
