namespace AirIQ.Services.Interfaces;

public interface IUpiAppLaunchService
{
    Task<UpiAppLaunchResult> LaunchAsync(string appKey);
}

public enum UpiAppLaunchStatus
{
    Success,
    NotInstalled,
    InvalidApp,
    LaunchFailed
}

public sealed record UpiAppLaunchResult(UpiAppLaunchStatus Status, string? ErrorMessage = null);
