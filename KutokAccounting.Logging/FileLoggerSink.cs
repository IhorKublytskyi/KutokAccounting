using KutokAccounting.Logging.Interfaces;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Logging;

public sealed class FileLoggerSink : ILogger
{
	private readonly IFileLogChannelHolder _channelHolder;
	private readonly string _categoryName;

	public FileLoggerSink(
		string categoryName,
		IFileLogChannelHolder channelHolder)
	{
		_categoryName = categoryName.Split('.').Last();
		_channelHolder = channelHolder;
	}

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull
	{
		return null;
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return true;
	}

	public void Log<TState>(LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter)
	{
		FileLoggerParameters parameters = new()
		{
			CategoryName = _categoryName,
			LogLevel = logLevel,
			Message = formatter(state, exception),
			Timestamp = DateTime.Now
		};

		_channelHolder.WriteToChannel(parameters);
	}
}