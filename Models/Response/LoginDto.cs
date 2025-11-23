using System;
using System.Text.Json.Serialization;

namespace AirIQ.Models.Response;

public record LoginDto
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    [JsonPropertyName("user")]
    public UserDto? User { get; set; }

    [JsonPropertyName("expiration")]
    public int Expiration { get; set; }
}
