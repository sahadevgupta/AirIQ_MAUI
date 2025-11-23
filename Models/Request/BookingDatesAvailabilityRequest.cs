using System;
using System.Text.Json.Serialization;

namespace AirIQ.Models.Request;

public class BookingDatesAvailabilityRequest
{
    [JsonPropertyName("origin")]
    public string? Origin { get; set; }

    [JsonPropertyName("destination")]
    public string? Destination { get; set; }

}
