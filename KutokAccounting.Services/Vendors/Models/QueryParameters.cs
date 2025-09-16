namespace KutokAccounting.Services.Vendors.DataTransferObjects;

public sealed record QueryParameters(string? Name, string? SearchString, Pagination Pagination);