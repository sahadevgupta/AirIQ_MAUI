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
                return $"{ConvertToFlagEmoji(Origin)} {Origin} - {parts[0].Trim()}";
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
                return $"{ConvertToFlagEmoji(Destination)} {Destination} - {parts[1].Trim()}";
            }
            return string.Empty;
        }
    }

    private string ConvertToFlagEmoji(string? countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
            return string.Empty;

        var a = string.Concat(countryCode
            .ToUpper()
            .Select(c => char.ConvertFromUtf32(0x1F1E6 - 'A' + c)));

        return string.Empty;
    }
}
