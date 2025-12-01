using System;
using System.Text.Json.Serialization;

namespace AirIQ.Models.Request;

public class InfantInfoRequest : PassengerRequest
{

    [JsonPropertyName("travel_with")]
    public string? TravelWith { get; set; }

}
