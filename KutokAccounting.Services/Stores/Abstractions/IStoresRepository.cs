using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IStoresRepository
{
	Task CreateStoreAsync(Store store, CancellationToken ct);
	int GetStoresCount();
	IQueryable<Store> GetStoresPage(int pageSize, int pageNumber);
	ValueTask UpdateStoreAsync(int storeId, Store updatedStore, CancellationToken ct);
	Task DeleteStoreAsync(int storeId, CancellationToken ct);
	Task<bool> StoreExists(Store store);
}