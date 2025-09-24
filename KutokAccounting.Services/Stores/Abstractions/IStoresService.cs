using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Dtos;
using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IStoresService
{
	ValueTask CreateStoreAsync(StoreDto storeDto, CancellationToken ct);

	ValueTask<PagedResult<StoreDto>> GetStoresPageAsync(Pagination pagination,
		SearchParameters? searchParameters, 
		CancellationToken ct);
	ValueTask UpdateStoreAsync(int storeId, StoreDto updatedStoreDto, CancellationToken ct);
	ValueTask DeleteStoreAsync(int storeId, CancellationToken ct);
}