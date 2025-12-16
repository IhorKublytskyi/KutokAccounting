using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Invoices.Interfaces;
using KutokAccounting.Services.Invoices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Services.Invoices;

public class InvoiceRepository : IInvoiceRepository
{
	private readonly SemaphoreSlim _semaphoreSlim;
	private readonly KutokDbContext _dbContext;
	private readonly ILogger<InvoiceRepository> _logger;

	public InvoiceRepository(
		[FromKeyedServices(KutokConfigurations.WriteOperationsSemaphore)]
		SemaphoreSlim semaphoreSlim,
		KutokDbContext dbContext,
		ILogger<InvoiceRepository> logger)
	{
		_semaphoreSlim = semaphoreSlim;
		_dbContext = dbContext;
		_logger = logger;
	}

	public async ValueTask<PagedResult<Invoice>> GetAsync(InvoiceQueryParameters parameters,
		CancellationToken cancellationToken)
	{
		IQueryable<Invoice> query = _dbContext.Invoices.AsNoTracking();

		try
		{
			Task<int> countTask = query.CountAsync(cancellationToken);

			List<Invoice> invoices = await query
				.Skip(parameters.Pagination.Skip)
				.Take(parameters.Pagination.PageSize)
				.Include(i => i.Status)
				.Include(i => i.Vendor)
				.Include(i => i.Store)
				.Select(i => new Invoice
				{
					Id = i.Id,
					CreatedAt = i.CreatedAt,
					Number = i.Number,
					Status = i.Status,
					Vendor = i.Vendor,
					Store = i.Store,
					StoreId = i.StoreId,
					VendorId = i.VendorId
				})
				.ToListAsync(cancellationToken);

			return new PagedResult<Invoice>
			{
				Items = invoices,
				Count = await countTask
			};
		}
		catch (Exception e)
		{
			_logger.LogWarning(e, "Failed to retrieve invoices with QueryParameterss: {QueryParameters}", parameters);

			throw;
		}
	}

	public async ValueTask<Invoice> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public async ValueTask CreateAsync(Invoice invoice, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await _semaphoreSlim.WaitAsync(cancellationToken);

		try
		{
			await _dbContext.Invoices.AddAsync(invoice, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Failed to create invoice with Number: {InvoiceNumber}",
				invoice.Number);

			throw;
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	public async ValueTask UpdateAsync(Invoice invoice, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}