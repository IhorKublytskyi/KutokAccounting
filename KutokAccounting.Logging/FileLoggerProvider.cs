using System.Collections.Concurrent;
using KutokAccounting.Logging.Interfaces;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Logging;

[ProviderAlias("FileLogger")]
public sealed class FileLoggerProvider : ILoggerProvider
{
	private readonly ConcurrentDictionary<string, FileLoggerSink> _loggers = new(StringComparer.OrdinalIgnoreCase);
	private readonly IFileLogChannelHolder _channel;

	public FileLoggerProvider(IFileLogChannelHolder channel)
	{
		_channel = channel;
	}

	public ILogger CreateLogger(string categoryName)
	{
		return _loggers.GetOrAdd(categoryName, name => new FileLoggerSink(name, _channel));
	}

	public void Dispose()
	{
		_loggers.Clear();
	}
}