namespace KutokAccounting.Logging.Interfaces;

public interface IFileLoggerProcessor
{
	Task ProcessAsync(CancellationToken cancellationToken);
}