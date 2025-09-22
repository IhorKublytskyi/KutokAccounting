using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IStoresRepository
{
	ValueTask CreateStoreAsync(Store store, CancellationToken ct);
	ValueTask<IEnumerable<Store>> GetFilteredPageOfStoresAsync(Page page,
		CancellationToken ct,
		SearchParameters? searchParameters = null);
	ValueTask<int> GetStoresCountAsync();
	ValueTask UpdateStoreAsync(int storeId, Store updatedStore, CancellationToken ct);
	ValueTask DeleteStoreAsync(int storeId, CancellationToken ct);
	
}