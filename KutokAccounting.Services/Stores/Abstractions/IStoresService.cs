using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Dtos;
using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IStoresService
{
	ValueTask CreateAsync(StoreDto storeDto, CancellationToken ct);

	ValueTask<PagedResult<StoreDto>> GetPageAsync(Pagination pagination, //pagination + searchParameters
		StoreSearchParameters? searchParameters, 
		CancellationToken ct);
	ValueTask UpdateAsync(int storeId, StoreDto updatedStoreDto, CancellationToken ct);
	ValueTask DeleteAsync(int storeId, CancellationToken ct);
}