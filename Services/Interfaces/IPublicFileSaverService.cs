namespace AirIQ.Services.Interfaces;

/// <summary>
///     Saves a generated file to a location the user can browse to outside the app -
///     the device's public Downloads folder on Android, and the app's Files-app-visible
///     Documents folder on iOS (Apple does not allow third-party apps to write directly
///     into the system Downloads location).
/// </summary>
public interface IPublicFileSaverService
{
    /// <summary>
    ///     Writes <paramref name="content" /> to the platform's user-accessible folder and
    ///     returns a human-readable description of where it was saved.
    /// </summary>
    Task<string> SaveToDownloadsAsync(string fileName, byte[] content, string contentType);
}
