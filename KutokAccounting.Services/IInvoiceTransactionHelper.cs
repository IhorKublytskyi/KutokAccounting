using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Invoices.Models;

namespace KutokAccounting.Services;

public interface IInvoiceTransactionHelper
{
	ValueTask<Invoice> OpenAsync(InvoiceDto request, CancellationToken cancellationToken);
}