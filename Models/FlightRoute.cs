using System;

namespace AirIQ.Models;

public class FlightRoute
{
    public string? Sector { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }

    public string OriginRoute
    {
        get
        {
            var parts = Sector?.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries);
            return parts?.Length > 0 ? $"{Origin} - {parts[0].Trim()}" : string.Empty;
        }
    }

    public string DestinationRoute
    {
        get
        {
            var parts = Sector?.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries);
            return parts?.Length > 0 ? $"{Destination} - {parts[1].Trim()}" : string.Empty;
        }
    }
}
