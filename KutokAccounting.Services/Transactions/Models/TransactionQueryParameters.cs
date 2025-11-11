using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.Transactions.Models;

public sealed record TransactionQueryParameters(Filters? Filters, string? SearchString, Pagination Pagination);