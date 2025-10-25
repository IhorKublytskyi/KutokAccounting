using KutokAccounting.Logging.Interfaces;

namespace KutokAccounting.Logging;

public sealed class FileLoggerProcessor : IFileLoggerProcessor
{
	private readonly IFileLogChannelHolder _channelHolder;

	public FileLoggerProcessor(IFileLogChannelHolder channelHolder)
	{
		_channelHolder = channelHolder;
	}

	public async Task ProcessAsync(CancellationToken cancellationToken)
	{
		await using LogFileWriter writer = new();

		await foreach (FileLoggerParameters parameters in _channelHolder.ReadFromChannelAsync(cancellationToken))
		{
			string log = string.Format(
				FileLoggerConfiguration.LogTemplate,
				parameters.Timestamp,
				FileLoggerConfiguration.LogLevelToString[parameters.LogLevel],
				parameters.CategoryName, parameters.Message);

			await writer.WriteAsync(log + Environment.NewLine, cancellationToken);
		}
	}
}