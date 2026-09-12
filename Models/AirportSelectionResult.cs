using AirIQ.Enums;

namespace AirIQ.Models;

public class AirportSelectionResult
{
    public AirportFieldType FieldType { get; set; }
    public FlightRoute? SelectedAirport { get; set; }
}
