using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IStoresRepository
{
	ValueTask CreateAsync(Store store, CancellationToken ct);

	ValueTask<PagedResult<Store>> GetFilteredPageAsync(
		StoreQueryParameters queryParameters,
		CancellationToken ct);

	ValueTask<Store?> GetByIdAsync(int id, CancellationToken ct);

	ValueTask UpdateAsync(int storeId,
		Store updatedStore,
		CancellationToken ct);

	ValueTask DeleteAsync(int storeId, CancellationToken ct);
}