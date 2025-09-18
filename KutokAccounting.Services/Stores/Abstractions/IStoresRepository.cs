using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IStoresRepository
{
	ValueTask CreateStoreAsync(Store store, CancellationToken ct);
	ValueTask<int> GetStoresCountAsync();
	IQueryable<Store> GetStoresPage(int pageSize, int pageNumber);
	ValueTask UpdateStoreAsync(int storeId, Store updatedStore, CancellationToken ct);
	ValueTask DeleteStoreAsync(int storeId, CancellationToken ct);
}