using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Transactions.Models;

public sealed record Filters
{
	public string? Name { get; set; }
	public string? Description { get; set; }
	public required int StoreId { get; set; }
	public int? TransactionTypeId { get; set; }
	public Money? Value { get; set; }
	public Money? MoreThan { get; set; }
	public Money? LessThan { get; set; }
	public DateTimeRange? Range { get; set; }
}