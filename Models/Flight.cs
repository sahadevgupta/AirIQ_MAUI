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
            var result = FlightNumber?.Split(" ");

            return $"https://airiq.in/img/airlinelogos/{result[0]}.png";
        }
    }

    public DateTime? DepartureDateTime
    {
        get
        {
            DateTime.TryParse(DepartureDate, out DateTime result);
            return result;
        }
    }

    public DateTime? ArrivalDateTime
    {
        get
        {
            DateTime.TryParse(DepartureDate, out DateTime result);
            return result;
        }
    }

    public string TotalDuration
    {
        get
        {
            DateTime departure = DateTime.Parse($"{DepartureDate} {DepartureTime}");
            DateTime arrival = DateTime.Parse($"{ArrivalDate} {ArrivalTime}");

            TimeSpan duration = arrival - departure;
            return $"{duration.Hours}hr {duration.Minutes}min";
        }
    }
}
