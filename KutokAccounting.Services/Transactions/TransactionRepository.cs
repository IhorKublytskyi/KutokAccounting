using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Transactions.Interfaces;
using KutokAccounting.Services.Transactions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Services.Transactions;

public sealed class TransactionRepository : ITransactionRepository
{
	private readonly KutokDbContext _dbContext;
	private readonly SemaphoreSlim _semaphoreSlim;
	private readonly ILogger<Transaction> _logger;

	public TransactionRepository(KutokDbContext dbContext,
		[FromKeyedServices(KutokConfigurations.WriteOperationsSemaphore)] SemaphoreSlim semaphoreSlim,
		ILogger<Transaction> logger)
	{
		_dbContext = dbContext;
		_semaphoreSlim = semaphoreSlim;
		_logger = logger;
	}

	public async ValueTask<PagedResult<Transaction>> GetAsync(TransactionQueryParameters parameters,
		CancellationToken cancellationToken)
	{
		IQueryable<Transaction> query = _dbContext.Transactions.AsNoTracking();

		if (parameters?.Filters?.Name is not null)
		{
			query = query.Where(t => t.Name == parameters.Filters.Name);
		}

		if (parameters?.Filters?.Description is not null)
		{
			query = query.Where(t => t.Description == parameters.Filters.Description);
		}

		if (parameters?.Filters?.TransactionTypeId is not null)
		{
			query = query.Where(t => t.TransactionTypeId == parameters.Filters.TransactionTypeId);
		}

		if (parameters?.Filters?.Value is not null)
		{
			query = query.Where(t => t.Money.Value == parameters.Filters.Value);
		}

		if (parameters?.Filters?.Range is not null)
		{
			query = query.Where(t => t.CreatedAt >= parameters.Filters.Range.Value.StartOfRange &&
				t.CreatedAt <= parameters.Filters.Range.Value.EndOfRange);
		}

		if (string.IsNullOrWhiteSpace(parameters?.SearchString) is false)
		{
			query = query.Where(t => EF.Functions.Like(t.Name + " " + t.Description, $"%{parameters.SearchString}%"));
		}

		try
		{
			Task<int> countTask = query.CountAsync(cancellationToken);

			List<Transaction> transactions = await query
				.Skip(parameters.Pagination.Skip)
				.Take(parameters.Pagination.PageSize)
				.Include(t => t.TransactionType)
				.Select(t => new Transaction
				{
					Id = t.Id,
					Name = t.Name,
					Description = t.Description,
					Money = new Money(t.Money.Value),
					CreatedAt = t.CreatedAt,
					TransactionType = t.TransactionType,
					TransactionTypeId = t.TransactionType.Id
				})
				.OrderBy(t => t.CreatedAt)
				.ToListAsync(cancellationToken);

			return new PagedResult<Transaction>
			{
				Items = transactions,
				Count = await countTask
			};
		}
		catch (Exception e)
		{
			_logger.LogWarning(e, "Failed to retrieve transaction types with QueryParameters: {QueryParameters}",
				parameters);

			throw;
		}
	}
	
	public async ValueTask<Transaction> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		try

		{
			Transaction? transaction = await _dbContext.Transactions
				.AsNoTracking()
				.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

			return transaction;
		}
		catch (Exception e)
		{
			_logger.LogWarning(e, "Failed to retrieve transaction with Id: {TransactionId}", id);

			throw;
		}
	}

	public async ValueTask CreateAsync(Transaction transaction, CancellationToken cancellationToken)
	{
		await _semaphoreSlim.WaitAsync(cancellationToken);

		try
		{
			await _dbContext.Transactions.AddAsync(transaction, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
		catch (Exception e)
		{
			_logger.LogInformation(e, "Failed to create transaction with Name: {TransactionName}",
				transaction.Name);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	public async ValueTask UpdateAsync(Transaction transaction, CancellationToken cancellationToken)
	{
		await _semaphoreSlim.WaitAsync(cancellationToken);

		try
		{
			await _dbContext.Transactions
				.Where(t => t.Id == transaction.Id)
				.ExecuteUpdateAsync(t => t
						.SetProperty(p => p.Name, transaction.Name)
						.SetProperty(p => p.Description, transaction.Description)
						.SetProperty(p => p.Money, transaction.Money)
						.SetProperty(p => p.TransactionTypeId, transaction.TransactionTypeId),
					cancellationToken);
		}
		catch (Exception e)
		{
			_logger.LogWarning(e,
				"Failed to update transaction with Id: {TransactionId}, Name: {TransactionName}",
				transaction.Id, transaction.Name);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
	{
		await _semaphoreSlim.WaitAsync(cancellationToken);

		try
		{
			await _dbContext.Transactions
				.Where(transaction => transaction.Id == id)
				.ExecuteDeleteAsync(cancellationToken);
		}
		catch (Exception e)
		{
			_logger.LogWarning(e, "Failed to delete transaction with Id: {TransactionId}", id);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}
}