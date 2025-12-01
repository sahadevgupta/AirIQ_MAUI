using System;
using AirIQ.Enums;

namespace AirIQ.Models.Response;

public record AuthInfoDto
{
    public string? AccessToken { get; set; }
    public DateTime AccessTokenExpiresOn { get; set; }
    public string? UserName { get; set; }
    public string? ApiKey { get; set; }
    public AuthInfoStatusType Status { get; set; }
}
