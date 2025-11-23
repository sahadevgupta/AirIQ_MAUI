using System;

namespace AirIQ.Models.Response;

public record FlightRouteDto
{
    public string? Sector { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }
}
