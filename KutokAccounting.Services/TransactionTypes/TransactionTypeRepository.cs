using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.TransactionTypes.Interfaces;
using KutokAccounting.Services.TransactionTypes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Services.TransactionTypes;

public class TransactionTypeRepository : ITransactionTypeRepository
{
    private readonly KutokDbContext _dbContext;
    private readonly SemaphoreSlim _semaphoreSlim;
    private readonly ILogger<TransactionTypeRepository> _logger;
    
    public TransactionTypeRepository(KutokDbContext dbContext, [FromKeyedServices(KutokConfigurations.WriteOperationsSemaphore)] SemaphoreSlim semaphoreSlim, ILogger<TransactionTypeRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _semaphoreSlim = semaphoreSlim;
    }
    public async ValueTask CreateAsync(TransactionType transactionType, CancellationToken cancellationToken)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            await _dbContext.TransactionTypes.AddAsync(transactionType, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "Failed to create transaction type with Name: {TransactionTypeName}", transactionType.Name);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async ValueTask<PagedResult<TransactionType>> GetAsync(QueryParameters queryParameters, CancellationToken cancellationToken)
    {
        IQueryable<TransactionType> query = _dbContext.TransactionTypes.AsNoTracking();

        if (string.IsNullOrWhiteSpace(queryParameters?.Filters?.Name) is false)
            query = query.Where(tp => tp.Name == queryParameters.Filters.Name);
        if(queryParameters?.Filters?.IsPositiveValue is not null)
            query = query.Where(tp => tp.IsPositiveValue == queryParameters.Filters.IsPositiveValue);
        if (string.IsNullOrWhiteSpace(queryParameters?.SearchString) is false)
            query = query.Where(tp => EF.Functions.Like(tp.Name, $"%{queryParameters.SearchString}%"));

        try
        {
            Task<int> countTask = query.CountAsync(cancellationToken);

            List<TransactionType> transactionTypes = await query
                .Skip(queryParameters.Pagination.Skip)
                .Take(queryParameters.Pagination.PageSize)
                .Select(tp => new TransactionType
                {
                    Id = tp.Id,
                    Name = tp.Name,
                    IsPositiveValue = tp.IsPositiveValue

                })
                .OrderBy(tp => tp.Name)
                .ToListAsync(cancellationToken);

            return new PagedResult<TransactionType>()
            {
                Items = transactionTypes,
                Count = await countTask
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to retrieve transaction types with QueryParameters: {QueryParameters}", queryParameters);

            throw;
        }
    }
    
    public async ValueTask<TransactionType?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            TransactionType? transactionType = await _dbContext.TransactionTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(tp => tp.Id == id, cancellationToken);

            return transactionType;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to retrieve transaction type with Id: {TransactionTypeId}", id);

            throw;
        }
    }
    
    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            await _dbContext.TransactionTypes
                .Where(transactionType => transactionType.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete transaction type with Id: {TransactionTypeId}", id);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async ValueTask UpdateAsync(TransactionType transactionType, CancellationToken cancellationToken)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            await _dbContext.TransactionTypes
                .Where(tp => tp.Id == transactionType.Id)
                .ExecuteUpdateAsync(tp => tp
                    .SetProperty(p => p.Name, transactionType.Name)
                    .SetProperty(p => p.IsPositiveValue, transactionType.IsPositiveValue), cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Failed to update transaction type with Id: {TransactionTypeId}, Name: {TransactionTypeName}",
                transactionType.Id, transactionType.Name);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
}