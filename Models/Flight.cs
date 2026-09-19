using System;
using System.Linq;

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
            var code = FlightNumber?.Split(' ').FirstOrDefault() ?? "default";

            return $"https://airiq.in/img/airlinelogos/{code}.png";
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
            DateTime.TryParse(ArrivalDate, out DateTime result);
            return result;
        }
    }

    public string TotalDuration
    {
        get
        {
            if (!DateTime.TryParse($"{DepartureDate} {DepartureTime}", out DateTime departure) ||
                !DateTime.TryParse($"{ArrivalDate} {ArrivalTime}", out DateTime arrival))
            {
                return "-";
            }

            TimeSpan duration = arrival - departure;
            return $"{duration.Hours}hr {duration.Minutes}min";
        }
    }
}
