using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.TransactionTypes.Models;

public sealed record QueryParameters(Filters? Filters, string? SearchString, Pagination Pagination);