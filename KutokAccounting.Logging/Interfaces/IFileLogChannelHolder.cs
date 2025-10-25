namespace KutokAccounting.Logging.Interfaces;

public interface IFileLogChannelHolder
{
	void WriteToChannel(FileLoggerParameters message);
	IAsyncEnumerable<FileLoggerParameters> ReadFromChannelAsync(CancellationToken cancellation);
}