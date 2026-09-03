namespace AirIQ.Models;

public class AirportListItem
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public FlightRoute Route { get; set; } = null!;

    public string Location => string.Join(", ", new[] { City, Country }.Where(s => !string.IsNullOrWhiteSpace(s)));
}
