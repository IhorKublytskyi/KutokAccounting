namespace KutokAccounting.Logging;

public static class LogFilesCleaner
{
	public static async Task CleanAsync(CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		DirectoryInfo directory = new(FileLoggerConfiguration.LoggingDirectoryPath);

		FileInfo[] files = directory.GetFiles();

		foreach (FileInfo file in files)
		{
			if (IsFileExpired(file, FileLoggerConfiguration.LogExpirationDays))
			{
				file.Delete();
			}
		}
	}

	private static bool IsFileExpired(FileInfo file, int logExpiration)
	{
		return DateTime.Today.AddDays(-7) >= file.CreationTime;
	}
}