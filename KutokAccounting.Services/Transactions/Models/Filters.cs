using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Transactions.Models;

public sealed record Filters
{
	public string? Name { get; set; }
	public string? Description { get; set; }
	public int? TransactionTypeId { get; set; }
	public int? Value { get; set; }
	public DateTimeRange? Range { get; set; }
}