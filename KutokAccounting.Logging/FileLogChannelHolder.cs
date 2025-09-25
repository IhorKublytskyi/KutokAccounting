using System.Runtime.CompilerServices;
using System.Threading.Channels;
using KutokAccounting.Logging.Interfaces;

namespace KutokAccounting.Logging;

public sealed class FileLogChannelHolder : IFileLogChannelHolder
{
	private readonly Channel<FileLoggerParameters> _channel;

	public FileLogChannelHolder()
	{
		_channel = Channel.CreateUnbounded<FileLoggerParameters>();
	}

	public void WriteToChannel(FileLoggerParameters message)
	{
		_channel.Writer.TryWrite(message);
	}

	public async IAsyncEnumerable<FileLoggerParameters> ReadFromChannelAsync(
		[EnumeratorCancellation] CancellationToken cancellationToken)
	{
		await foreach (FileLoggerParameters item in _channel.Reader.ReadAllAsync(CancellationToken.None))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}

			yield return item;
		}
	}
}