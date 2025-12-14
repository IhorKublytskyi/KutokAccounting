using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.Invoices.Models;

public sealed record InvoiceDto
{
	public required string Number { get; init; }
	public required int StoreId { get; init; }
	public required int VendorId { get; init; }
	public required long Value { get; set; }
}