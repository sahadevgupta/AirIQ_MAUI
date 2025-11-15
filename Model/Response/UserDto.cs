using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AirIQ.Model.Response;

public record UserDto
{
    [JsonPropertyName("agency_id")]
    public int AgencyId { get; set; }

    [JsonPropertyName("agency_name")]
    public string? AgencyName { get; set; }

    [JsonPropertyName("contact_person")]
    public string? ContactPerson { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("balance")]
    public double Balance { get; set; }

    [JsonPropertyName("email_id")]
    public string? EmailId { get; set; }

    [JsonPropertyName("mobile_no")]
    public string? MobileNumber { get; set; }
}
