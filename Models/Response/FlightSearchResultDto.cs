using System;
using System.Text.Json.Serialization;

namespace AirIQ.Models.Response;

public record FlightSearchResultDto
{
    [JsonPropertyName("ticket_id")]
    public string? TicketId { get; set; }

    [JsonPropertyName("origin")]
    public string? Origin { get; set; }

    [JsonPropertyName("destination")]
    public string? Destination { get; set; }

    [JsonPropertyName("airline")]
    public string? Airline { get; set; }

    [JsonPropertyName("departure_date")]
    public string? DepartureDate { get; set; }

    [JsonPropertyName("departure_time")]
    public string? DepartureTime { get; set; }

    [JsonPropertyName("arival_time")]
    public string? ArrivalTime { get; set; }

    [JsonPropertyName("arival_date")]
    public string? ArrivalDate { get; set; }

    [JsonPropertyName("flight_number")]
    public string? FlightNumber { get; set; }

    [JsonPropertyName("flight_route")]
    public string? FlightRoute { get; set; }

    [JsonPropertyName("pax")]
    public int Pax { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("infant_price")]
    public double InfantPrice { get; set; }

    [JsonPropertyName("inventory_type")]
    public string? InventoryType { get; set; }

    [JsonPropertyName("cabin_baggage")]
    public string? CabinBaggage { get; set; }

    [JsonPropertyName("hand_luggage")]
    public string? HandLuggage { get; set; }

    [JsonPropertyName("isinternational")]
    public bool Isinternational { get; set; }
}
