using System;
using System.Text.Json.Serialization;

namespace AirIQ.Model.Request;

public class LoginRequest
{
    [JsonPropertyName("Username")]
    public string? UserName { get; set; }

    [JsonPropertyName("Password")]
    public string? Password { get; set; }
}
