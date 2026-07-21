using AirIQ.Extensions;

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
                return $"{parts[0].Trim()} ({Origin})";
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
                return $"{parts[1].Trim()} ({Destination})";
            }
            return string.Empty;
        }
    }

    private string ConvertToFlagEmoji(string? iataCode)
    {
        if (string.IsNullOrWhiteSpace(iataCode))
            return string.Empty;

        var country = iataCode.GetCountry();
        return country!.GetFlagEmoji() ?? string.Empty;
    }
}
