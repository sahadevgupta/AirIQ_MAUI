using System;

namespace AirIQ.Model.Response;

public record FlightRouteDto
{
    public string? Sector { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }
}
