using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.Transactions.Models;

public record TransactionCalculationView
{
	public Money Money { get; set; }
	public bool Sign { get; set; }
}