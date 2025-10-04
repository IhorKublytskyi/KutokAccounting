namespace KutokAccounting.Services.TransactionTypes.Exceptions;

public class NotFoundException : Exception
{
	public NotFoundException(string message) : base(message)
	{
	}
}