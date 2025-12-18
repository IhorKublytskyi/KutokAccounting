using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Invoices.Models;
using KutokAccounting.Services.Invoices.Validators;

namespace KutokAccounting.Services.Invoices.Interfaces;

public interface IInvoiceStateService
{
	ValueTask<Invoice> OpenInvoiceAsync(InvoiceDto request, CancellationToken cancellationToken);
	ValueTask CloseInvoiceAsync(CloseInvoiceDto request, CancellationToken cancellationToken);
}