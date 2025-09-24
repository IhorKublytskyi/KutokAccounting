using Microsoft.Extensions.Logging;

namespace KutokAccounting.Services.Logging;

public sealed class FileLoggerConfiguration
{
    public int EventId { get; set; }
    public string LoggingDirectoryPath => Path.Combine(Directory.GetCurrentDirectory(), "logs");
    public string LogTemplate => "[{0}] - [{1}] - [{2}] - [{3}]: {4}";
    public string LoggingFilePath => string.Format("{0}_logs.log", $"{DateTime.Now.ToShortDateString()}");
    public int LogExpirationDays => 7;

    public Dictionary<LogLevel, string> LogLevelToString = new()
    {
        //TODO: trace, debug
        [LogLevel.Information] = "Info",
        [LogLevel.Warning] = "Warn",
        [LogLevel.Error] = "Error",
        [LogLevel.Critical] = "Crit",
    };
}
