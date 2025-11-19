using System;
using Newtonsoft.Json;

namespace AirIQ.Model.Request;

public class FlightSearchRequest
{
    [JsonProperty("origin")]
    public string? Origin { get; set; }

    [JsonProperty("destination")]
    public string? Destination { get; set; }

    [JsonProperty("departure_date")]
    public string? DepartureDate { get; set; }

    [JsonProperty("adult")]
    public int Adult { get; set; }

    [JsonProperty("child")]
    public int Child { get; set; }

    [JsonProperty("infant")]
    public int Infant { get; set; }

    [JsonProperty("airline_code")]
    public string? AirlineCode { get; set; }
}
