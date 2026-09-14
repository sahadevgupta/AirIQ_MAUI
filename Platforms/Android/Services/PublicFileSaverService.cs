using AirIQ.Services.Interfaces;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Provider;
using AndroidEnvironment = Android.OS.Environment;

namespace AirIQ.Platforms.Services;

public class PublicFileSaverService : IPublicFileSaverService
{
    public async Task<string> SaveToDownloadsAsync(string fileName, byte[] content, string contentType)
    {
        var context = global::Android.App.Application.Context;

        // Scoped storage (Android 10+) lets any app insert into the shared Downloads
        // collection via MediaStore without requesting a storage permission.
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
        {
            var resolver = context.ContentResolver ?? throw new IOException("Content resolver unavailable.");

            var values = new ContentValues();
            values.Put(MediaStore.MediaColumns.DisplayName, fileName);
            values.Put(MediaStore.MediaColumns.MimeType, contentType);
            values.Put(MediaStore.MediaColumns.RelativePath, AndroidEnvironment.DirectoryDownloads);

            var uri = resolver.Insert(MediaStore.Downloads.ExternalContentUri!, values)
                ?? throw new IOException("Unable to create the file in Downloads.");

            await using (var outputStream = resolver.OpenOutputStream(uri) ?? throw new IOException("Unable to open the Downloads file for writing."))
            {
                await outputStream.WriteAsync(content, 0, content.Length);
            }

            return AndroidEnvironment.DirectoryDownloads!;
        }

        // Pre-scoped-storage devices need the legacy runtime permission and a direct
        // write into the public Downloads directory.
        var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
        if (status != PermissionStatus.Granted)
            throw new UnauthorizedAccessException("Storage permission was denied.");

        var downloadsDir = AndroidEnvironment.GetExternalStoragePublicDirectory(AndroidEnvironment.DirectoryDownloads)!.AbsolutePath;
        Directory.CreateDirectory(downloadsDir);

        var filePath = Path.Combine(downloadsDir, fileName);
        await File.WriteAllBytesAsync(filePath, content);

        // Make the new file immediately visible in the Downloads/Files apps.
        MediaScannerConnection.ScanFile(context, new[] { filePath }, null, null);

        return downloadsDir;
    }
}
