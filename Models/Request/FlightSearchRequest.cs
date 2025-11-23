using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AirIQ.Models.Request;

public class FlightSearchRequest
{
    [JsonPropertyName("origin")]
    public string? Origin { get; set; }

    [JsonPropertyName("destination")]
    public string? Destination { get; set; }

    [JsonPropertyName("departure_date")]
    public string? DepartureDate { get; set; }

    [JsonPropertyName("adult")]
    public int Adult { get; set; }

    [JsonPropertyName("child")]
    public int Child { get; set; }

    [JsonPropertyName("infant")]
    public int Infant { get; set; }

    [JsonPropertyName("airline_code")]
    public string? AirlineCode { get; set; }
}
