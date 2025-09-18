using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KutokAccounting.Services.Stores;

public class StoresRepository : IStoresRepository
{
	private readonly KutokDbContext _dbContext;
	private readonly SemaphoreSlim _semaphoreSlim;
	public StoresRepository(KutokDbContext dbContext, SemaphoreSlim semaphoreSlim)
	{
		_semaphoreSlim = semaphoreSlim;
		_dbContext = dbContext;
	}
	public async ValueTask CreateStoreAsync(Store store, CancellationToken ct)
	{
		var storeExists = await StoreExists(store.Id);
		if (storeExists)
		{
			throw new ArgumentException($"Store {store.Name} already exists");
		}
		
		await _semaphoreSlim.WaitAsync(ct);

		try
		{
			_dbContext.Stores.Add(store);
			await _dbContext.SaveChangesAsync(ct);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
		
	}
	
	public async ValueTask<int> GetStoresCountAsync()
	{
		return await _dbContext.Stores.CountAsync();
	}
	public IQueryable<Store> GetStoresPage(int pageSize, int pageNumber)
	{
		_dbContext.Stores.AsNoTracking();
		var startPosition = pageSize * (pageNumber - 1);
		return _dbContext.Stores.Skip(startPosition).Take(pageSize);
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
			var store = await _dbContext.Stores.FindAsync(storeId);
		
			ThrowIfStoreIsNull(store, storeId);
			
			//tech fields like history remain the same
			store.Name = updatedStore.Name;
			store.IsOpened = updatedStore.IsOpened;
			store.SetupDate = updatedStore.SetupDate;
			store.Address = updatedStore.Address;

			await _dbContext.Stores
				.Where(s => s.Id == store.Id)
				.ExecuteUpdateAsync(s => s.SetProperty(sProp => sProp, storeValue => store), ct);
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
			ThrowIfStoreIsNull(store, storeId);

			await _dbContext.Stores.Where(s => s.Id == storeId).ExecuteDeleteAsync(ct);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
		
	}

	private async ValueTask<bool> StoreExists(int storeId)
	{
		return await _dbContext.Stores.AnyAsync(s => s.Id == storeId);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="store"></param>
	/// <param name="storeId"></param>
	/// <exception cref="ArgumentException"></exception>
	private void ThrowIfStoreIsNull(Store? foundStore, int storeId)
	{
		if (foundStore is null)
		{
			throw new ArgumentException($"Store with id {storeId} does not exist");
		}
	}
}