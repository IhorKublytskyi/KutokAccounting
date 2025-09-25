using Microsoft.Extensions.Logging;

namespace KutokAccounting.Logging;

public static class FileLoggerConfiguration
{
	public static Dictionary<LogLevel, string> LogLevelToString = new()
	{
		[LogLevel.Debug] = "Debug",
		[LogLevel.Trace] = "Trace",
		[LogLevel.Information] = "Info",
		[LogLevel.Warning] = "Warn",
		[LogLevel.Error] = "Error"
	};

	public static string LoggingDirectoryPath => Path.Combine(Directory.GetCurrentDirectory(), "Logs");

	public static string LogTemplate => "[{0:HH:mm:ss.fff}]\t[{1}]\t[{2}]\t[{3}]";

	public static int LogExpirationDays => 7;
}