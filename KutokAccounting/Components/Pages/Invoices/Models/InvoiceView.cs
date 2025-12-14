using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Components.Pages.Invoices.Models;

public sealed record InvoiceView
{
	public required string Number { get; set; }
	public required	InvoiceStatus Status { get; set; }
	public required string VendorName { get; set; }
}