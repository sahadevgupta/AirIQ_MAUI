using AirIQ.Services.Interfaces;

namespace AirIQ.Platforms.Services;

public class PublicFileSaverService : IPublicFileSaverService
{
    public async Task<string> SaveToDownloadsAsync(string fileName, byte[] content, string contentType)
    {
        // iOS sandboxes apps away from a shared Downloads folder - the closest
        // user-accessible equivalent is the app's Documents folder, which shows up in
        // the Files app under "On My iPhone/iPad > AIRiQ" (see UIFileSharingEnabled /
        // LSSupportsOpeningDocumentsInPlace in Info.plist).
        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        Directory.CreateDirectory(documentsPath);

        var filePath = Path.Combine(documentsPath, fileName);
        await File.WriteAllBytesAsync(filePath, content);

        return documentsPath;
    }
}
