using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.Vendors.Models;

public sealed record VendorQueryParameters(
	string? Name,
	string? SearchString,
	Pagination Pagination);