using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.Vendors.DataTransferObjects;

public sealed record VendorPagedResult
{
    public required IEnumerable<Vendor>? Vendors { get; set; }
    public required int Count { get; set; }
}