using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.TransactionTypes.Models;

public sealed record TransactionTypeQueryParameters(
	Filters? Filters,
	string? SearchString,
	Pagination Pagination);