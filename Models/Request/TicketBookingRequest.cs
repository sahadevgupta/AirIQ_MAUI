using System;
using System.Text.Json.Serialization;

namespace AirIQ.Models.Request;

public class TicketBookingRequest
{
    [JsonPropertyName("ticket_id")]
    public string? TicketId { get; set; }

    [JsonPropertyName("total_pax")]
    public string? TotalPax { get; set; }

    [JsonPropertyName("adult")]
    public string? Adult { get; set; }

    [JsonPropertyName("child")]
    public string? Child { get; set; }

    [JsonPropertyName("infant")]
    public string? Infant { get; set; }

    [JsonPropertyName("adult_info")]
    public List<PassengerRequest>? AdultInfo { get; set; }

    [JsonPropertyName("child_info")]
    public List<PassengerRequest>? ChildInfo { get; set; }

    [JsonPropertyName("infant_info")]
    public List<InfantInfoRequest>? InfantInfo { get; set; }

}
