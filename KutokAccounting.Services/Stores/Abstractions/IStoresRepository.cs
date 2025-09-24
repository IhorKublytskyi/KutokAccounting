using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IStoresRepository
{
	ValueTask CreateStoreAsync(Store store, CancellationToken ct);
	ValueTask<PagedResult<Store>> GetFilteredPageOfStoresAsync(
		Pagination pagination,
		SearchParameters? searchParameters,
		CancellationToken ct);
	ValueTask UpdateStoreAsync(int storeId, Store updatedStore, CancellationToken ct);
	ValueTask DeleteStoreAsync(int storeId, CancellationToken ct);
	
}