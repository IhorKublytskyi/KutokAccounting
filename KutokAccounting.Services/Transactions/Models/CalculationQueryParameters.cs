using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Transactions.Models;

public record CalculationQueryParameters
{
	public DateTimeRange? Range { get; init; }
	public required int StoreId { get; init; }
}