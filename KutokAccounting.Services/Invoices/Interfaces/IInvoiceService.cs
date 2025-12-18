using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Invoices.Models;
using KutokAccounting.Services.Invoices.Validators;

namespace KutokAccounting.Services.Invoices.Interfaces;

public interface IInvoiceService
{
    ValueTask<PagedResult<Invoice>> GetAsync(InvoiceQueryParameters parameters, CancellationToken cancellationToken);
    ValueTask<Invoice> GetByIdAsync(int id, CancellationToken cancellationToken);
    ValueTask<Invoice> CreateAsync(InvoiceDto request, CancellationToken cancellationToken);
    ValueTask UpdateAsync(InvoiceDto request, CancellationToken cancellationToken);
    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
    ValueTask CloseAsync(CloseInvoiceDto request, CancellationToken cancellationToken);
}
