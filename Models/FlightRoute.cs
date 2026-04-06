using System;

namespace AirIQ.Models;

public class FlightRoute
{
    public string? Sector { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }

    public string? OriginAiportName { get; set; }
    public string? DestinationAiportName { get; set; }

    public string OriginRoute
    {
        get
        {
            var parts = Sector?.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts?.Length > 0)
            {
                OriginAiportName = parts[0].Trim();
                return $"{Origin} - {parts[0].Trim()}";
            }

            return string.Empty;
        }
    }

    public string DestinationRoute
    {
        get
        {
            var parts = Sector?.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts?.Length > 0)
            {
                DestinationAiportName = parts[1].Trim();
                return $"{Destination} - {parts[1].Trim()}";
            }
            return string.Empty;
        }
    }
}
