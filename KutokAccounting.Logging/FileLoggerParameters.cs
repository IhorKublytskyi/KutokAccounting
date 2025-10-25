using Microsoft.Extensions.Logging;

namespace KutokAccounting.Logging;

public record struct FileLoggerParameters
{
	public LogLevel LogLevel { get; set; }
	public string CategoryName { get; set; }
	public DateTime Timestamp { get; set; }
	public string Message { get; set; }
}