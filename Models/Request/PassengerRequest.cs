using System;
using System.Text.Json.Serialization;

namespace AirIQ.Models.Request;

public class PassengerRequest
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("passport_number")]
    public string? PassportNumber { get; set; }

    [JsonPropertyName("dob")]
    public string? Dob { get; set; }

    [JsonPropertyName("passport_expirydate")]
    public string? PassportExpirydate { get; set; }

    [JsonPropertyName("passport_issuing_country_code")]
    public string? PassportIssuingCountryCode { get; set; }

    [JsonPropertyName("nationality")]
    public string? Nationality { get; set; }

}
