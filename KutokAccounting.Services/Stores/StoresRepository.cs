using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Abstractions;
using KutokAccounting.Services.Stores.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Services.Stores;

public class StoresRepository : IStoresRepository
{
	private readonly KutokDbContext _dbContext;
	private readonly SemaphoreSlim _semaphoreSlim;
	private readonly IQueryBuilder _queryBuilder;
	private readonly ILogger<StoresRepository> _logger;

	public StoresRepository(KutokDbContext dbContext,
		[FromKeyedServices(KutokConfigurations.WriteOperationsSemaphore)]
		SemaphoreSlim semaphoreSlim,
		IQueryBuilder queryBuilder,
		ILogger<StoresRepository> logger)
	{
		_semaphoreSlim = semaphoreSlim;
		_queryBuilder = queryBuilder;
		_logger = logger;
		_dbContext = dbContext;
	}

	public async ValueTask CreateStoreAsync(Store store, CancellationToken ct)
	{
		await _semaphoreSlim.WaitAsync(ct);

		try
		{
			await _dbContext.Stores.AddAsync(store, ct);
			await _dbContext.SaveChangesAsync(ct);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	public async ValueTask<PagedResult<Store>> GetFilteredPageOfStoresAsync(
		StoreSearchParameters searchParameters,
		CancellationToken ct)
	{
		IQueryable<Store> getAllStoresQuery = _dbContext.Stores.AsNoTracking();
		IQueryable<Store> storesQuery = _queryBuilder.FilterStoresByParametersQuery(getAllStoresQuery, searchParameters);
		List<Store> pagedStores = await GetStoresPageAsync(storesQuery, searchParameters.Pagination, ct);
		int filteredStoresCount = await storesQuery.CountAsync(ct);

		return new PagedResult<Store>
		{
			Items = pagedStores,
			Count = filteredStoresCount
		};
	}

	public async ValueTask UpdateStoreAsync(int storeId,
		Store updatedStore,
		CancellationToken ct)
	{
		await _semaphoreSlim.WaitAsync(ct);

		try
		{
			await _dbContext.Stores
				.Where(s => s.Id == storeId)
				.ExecuteUpdateAsync(s => s
					.SetProperty(st => st.SetupDate, updatedStore.SetupDate)
					.SetProperty(st => st.Address, updatedStore.Address)
					.SetProperty(st => st.Name, updatedStore.Name)
					.SetProperty(st => st.IsOpened, updatedStore.IsOpened), ct);

			_logger.LogInformation("Store with {storeId} was updated", storeId);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	public async ValueTask DeleteStoreAsync(int storeId, CancellationToken ct)
	{
		await _semaphoreSlim.WaitAsync(ct);

		try
		{
			await _dbContext.Stores.Where(s => s.Id == storeId).ExecuteDeleteAsync(ct);
			_logger.LogInformation("Store with {storeId} was deleted", storeId);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	private async ValueTask<List<Store>> GetStoresPageAsync(IQueryable<Store> stores,
		Pagination pagination,
		CancellationToken ct)
	{
		IOrderedQueryable<Store> query = stores
			.Select(s => new Store
			{
				Id = s.Id,
				Name = s.Name,
				IsOpened = s.IsOpened,
				SetupDate = s.SetupDate,
				Address = s.Address
			})
			.Skip(pagination.Skip)
			.Take(pagination.PageSize)
			.OrderByDescending(s => s.SetupDate);

		return await query.ToListAsync(ct);
	}
}