using System;

namespace AirIQ.Models;

public class Flight
{
    public string? TicketId { get; set; }

    public string? Origin { get; set; }

    public string? Destination { get; set; }

    public string? Airline { get; set; }

    public string? DepartureDate { get; set; }

    public string? DepartureTime { get; set; }

    public string? ArrivalTime { get; set; }

    public string? ArrivalDate { get; set; }

    public string? FlightNumber { get; set; }

    public string? FlightRoute { get; set; }

    public int Pax { get; set; }

    public double Price { get; set; }

    public double InfantPrice { get; set; }

    public string? InventoryType { get; set; }

    public string? CabinBaggage { get; set; }

    public string? HandLuggage { get; set; }

    public bool Isinternational { get; set; }

    public string? AirlineLogoUrl
    {
        get
        {
            var airline = Airline?.Replace(" ", "");

            return $"https://logo.clearbit.com/{airline}.com";
        } 
    }
}
