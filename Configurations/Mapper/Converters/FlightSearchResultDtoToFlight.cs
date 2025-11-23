using System;
using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters;

public class FlightSearchResultDtoToFlight : ConverterBase<FlightSearchResultDto, Flight>
{
    protected override Flight ConvertImpl(FlightSearchResultDto source)
    {
        return new Flight
        {
            Airline = source.Airline,
            ArrivalDate = source.ArrivalDate,
            ArrivalTime = source.ArrivalTime,
            CabinBaggage = source.CabinBaggage,
            DepartureDate = source.DepartureDate,
            DepartureTime = source.DepartureTime,
            Destination = source.Destination,
            FlightNumber = source.FlightNumber,
            FlightRoute = source.FlightRoute,
            InfantPrice = source.InfantPrice,
            InventoryType = source.InventoryType,
            Origin = source.Origin,
            Pax = source.Pax,
            Price = source.Price,
            TicketId = source.TicketId,
            HandLuggage = source.HandLuggage,
            Isinternational = source.Isinternational
        };
    }
}
