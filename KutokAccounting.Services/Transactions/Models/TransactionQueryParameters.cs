using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.Transactions.Models;
public sealed record TransactionQueryParameters
{
	public Filters? Filters { get; init; }
	public Sorting? Sorting { get; init; }
	public string? SearchString { get; init; }
	public Pagination Pagination { get; init; }
}