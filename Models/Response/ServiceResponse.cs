using System;
using System.Text.Json.Serialization;

namespace AirIQ.Models.Response;

public sealed class ServiceResponse<T>
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }
    
    [JsonPropertyName("code")]
    public string? ErrorCode { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
