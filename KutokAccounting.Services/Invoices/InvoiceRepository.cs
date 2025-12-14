using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Invoices.Interfaces;
using KutokAccounting.Services.Invoices.Models;

namespace KutokAccounting.Services.Invoices;

public class InvoiceRepository : IInvoiceRepository
{
	public async ValueTask<PagedResult<Invoice>> GetAsync(InvoiceQueryParameters parameters, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public async ValueTask<Invoice> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public async ValueTask CreateAsync(Invoice request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public async ValueTask UpdateAsync(Invoice request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}