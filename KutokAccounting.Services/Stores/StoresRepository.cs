using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Abstractions;
using KutokAccounting.Services.Stores.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KutokAccounting.Services.Stores;

public class StoresRepository : IStoresRepository
{
	private readonly KutokDbContext _dbContext;
	private readonly SemaphoreSlim _semaphoreSlim;
	private readonly IGetStoresQueryBuilder _getStoresQueryBuilder;

	public StoresRepository(KutokDbContext dbContext,
		[FromKeyedServices(KutokConfigurations.WriteOperationsSemaphore)]
		SemaphoreSlim semaphoreSlim,
		IGetStoresQueryBuilder getStoresQueryBuilder)
	{
		_semaphoreSlim = semaphoreSlim;
		_getStoresQueryBuilder = getStoresQueryBuilder;
		_dbContext = dbContext;
	}

	public async ValueTask CreateAsync(Store store, CancellationToken ct)
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

	public async ValueTask<PagedResult<Store>> GetFilteredPageAsync(
		StoreQueryParameters queryParameters,
		CancellationToken ct)
	{
		IQueryable<Store> storesQuery = _getStoresQueryBuilder
			.EmptyPreviousQuery()
			.SearchName(queryParameters.Name)
			.SearchAddress(queryParameters.Address)
			.SearchSetupDate(queryParameters.SetupDate)
			.SearchOpened(queryParameters.IsOpened)
			.BuildQuery();
		
		Task<int> filteredStoresCountTask = storesQuery.CountAsync(ct);
		List<Store> pagedStores = await GetPageAsync(storesQuery, queryParameters.Pagination, ct);

		return new PagedResult<Store>
		{
			Items = pagedStores,
			Count = await filteredStoresCountTask
		};
	}

	public async ValueTask UpdateAsync(int storeId,
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
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	public async ValueTask DeleteAsync(int storeId, CancellationToken ct)
	{
		await _semaphoreSlim.WaitAsync(ct);

		try
		{
			await _dbContext.Stores.Where(s => s.Id == storeId).ExecuteDeleteAsync(ct);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	private async ValueTask<List<Store>> GetPageAsync(IQueryable<Store> stores,
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