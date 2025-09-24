using System.Diagnostics;

namespace KutokAccounting.Services.Logging;

public static class LogFilesCleaner
{
    private static readonly FileLoggerConfiguration config = new();
    public static async Task CleanAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        DirectoryInfo directory = new DirectoryInfo(config.LoggingDirectoryPath);

        FileInfo[] files = directory.GetFiles();

        foreach (var file in files)
        {
            if (IsFileExpired(file, config.LogExpirationDays))
            {
                file.Delete();
            }
        }
    }

    private static bool IsFileExpired(FileInfo file, int logExpiration)
        => (DateTime.Today.AddDays(-7) >= file.CreationTime);
}
