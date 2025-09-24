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
	private readonly IStoreBuilder _storeBuilder;
	private readonly ILogger<StoresRepository> _logger;
	public StoresRepository(KutokDbContext dbContext, [FromKeyedServices(KutokConfigurations.WriteOperationsSemaphore)] SemaphoreSlim semaphoreSlim,
		IStoreBuilder storeBuilder,
		ILogger<StoresRepository> logger)
	{
		_semaphoreSlim = semaphoreSlim;
		_storeBuilder = storeBuilder;
		_logger = logger;
		_dbContext = dbContext;
	}
	public async ValueTask CreateStoreAsync(Store store, CancellationToken ct)
	{
		var storeExists = await StoreExists(store.Id);
		if (storeExists)
		{
			_logger.LogWarning("Store with {storeId} already exists}", store.Id);
			throw new ArgumentException($"Store {store.Name} already exists");
		}
		
		await _semaphoreSlim.WaitAsync(ct);

		try
		{
			_dbContext.Stores.Add(store);
			await _dbContext.SaveChangesAsync(ct);
			
			_logger.LogInformation("New store created with following properties. Name: {storeName}, Address: {storeAddress}, Is opened: {isOpened}, Setup date: {setUpDate}"
				, store.Name, store.Address, store.IsOpened, store.SetupDate);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	private async ValueTask<List<Store>> GetStoresPageAsync(IQueryable<Store> stores, Pagination pagination, CancellationToken ct)
	{
		var startPosition = pagination.PageSize * (pagination.Page - 1);
		var query = stores
			.Select(s => new Store
			{
				Id = s.Id,
				Name = s.Name,
				IsOpened = s.IsOpened,
				SetupDate = s.SetupDate,
				Address = s.Address,
			})
			.Skip(startPosition)
			.Take(pagination.PageSize)
			.OrderByDescending(s => s.SetupDate);
		
		return await query.ToListAsync(ct);
	}
	public async ValueTask<PagedResult<Store>> GetFilteredPageOfStoresAsync(Pagination pagination, SearchParameters? searchParameters, CancellationToken ct)
	{
		var getAllStoresQuery = _dbContext.Stores.AsNoTracking();
		var query = _storeBuilder.GetQuery(getAllStoresQuery, searchParameters);
		var stores = await GetStoresPageAsync(query, pagination, ct);
		var count = await getAllStoresQuery.CountAsync(ct);
		
		return new PagedResult<Store>()
		{
			Items = stores,
			Count = count,
		};
	}
	
	/// <summary>
	/// updates store without the need to save changes manually
	/// </summary>
	/// <param name="storeId">store id to update</param>
	/// <param name="updatedStore">new characteristics of store</param>
	/// <param name="ct"></param>
	public async ValueTask UpdateStoreAsync(int storeId, Store updatedStore, CancellationToken ct)
	{
		await _semaphoreSlim.WaitAsync(ct);
		try
		{
			var storeExists = await StoreExists(storeId);
			
			if (storeExists is false)
			{
				_logger.LogWarning("Store with {storeId} does not exists", storeId);

				throw new NullReferenceException($"Store {storeId} does not exist");
			}
			
			//tech fields like history remain the same
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
			var store = _dbContext.Stores.Find(storeId);
			
			if (store is null)
			{
				_logger.LogWarning("Store with {storeId} does not exists", storeId);
				throw new ArgumentException($"Store with id {storeId} does not exist");
			}
			await _dbContext.Stores.Where(s => s.Id == storeId).ExecuteDeleteAsync(ct);
			_logger.LogInformation("Store with {storeId} was deleted", storeId);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
		
	}

	private async ValueTask<bool> StoreExists(int storeId)
	{
		
		return await _dbContext.Stores
			.AsNoTracking()
			.AnyAsync(s => s.Id == storeId);
	}

	
}