using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.Vendors.Models;

public sealed record QueryParameters(string? Name, string? SearchString, Pagination Pagination);