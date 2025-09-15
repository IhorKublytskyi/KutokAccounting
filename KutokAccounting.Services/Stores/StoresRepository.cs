using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KutokAccounting.Services.Stores;

public class StoresRepository : IStoresRepository
{
	private readonly KutokDbContext _dbContext;
	private readonly DbSet<Store> _storesDbSet;
	private readonly SemaphoreSlim _semaphoreSlim;
	public StoresRepository(KutokDbContext dbContext, SemaphoreSlim semaphoreSlim)
	{
		_semaphoreSlim = semaphoreSlim;
		_dbContext = dbContext;
		_storesDbSet = dbContext.Stores;
	}
	public async Task CreateStoreAsync(Store store, CancellationToken ct)
	{
		var storeExists = await StoreExists(store);
		if (storeExists)
		{
			throw new ArgumentException($"Store {store.Name} already exists");
		}
		
		await _semaphoreSlim.WaitAsync(ct);

		try
		{
			_storesDbSet.Add(store);
			await _dbContext.SaveChangesAsync(ct);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
		
	}
	
	public int GetStoresCount()
	{
		return _storesDbSet.Count();
	}
	public IQueryable<Store> GetStoresPage(int pageSize, int pageNumber)
	{
		_storesDbSet.AsNoTracking();
		var startPosition = pageSize * (pageNumber - 1);
		return _storesDbSet.Skip(startPosition).Take(pageSize);
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
			var store = await _storesDbSet.FindAsync(storeId);
		
			ThrowIfStoreIsNull(store, storeId);
			
			//tech fields like history remain the same
			store.Name = updatedStore.Name;
			store.IsOpened = updatedStore.IsOpened;
			store.SetupDate = updatedStore.SetupDate;
			store.Address = updatedStore.Address;
		
			_storesDbSet.Update(store);
			await _dbContext.SaveChangesAsync(ct);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
		
	}

	public async Task DeleteStoreAsync(int storeId, CancellationToken ct)
	{
		await _semaphoreSlim.WaitAsync(ct);

		try
		{
			var store = _storesDbSet.Find(storeId);
			ThrowIfStoreIsNull(store, storeId);
		
			_storesDbSet.Remove(store);
			await _dbContext.SaveChangesAsync(ct);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
		
	}

	public async Task<bool> StoreExists(Store store)
	{
		return await _storesDbSet.AnyAsync(s => s.Id == store.Id);
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