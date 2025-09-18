using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Dtos;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IStoresService
{
	ValueTask CreateStoreAsync(StoreDto storeDto, CancellationToken ct);
	IQueryable<StoreDto> GetStoresPageAsync(int pageSize, int pageNumber);
	ValueTask<int> GetAllStoresCountAsync();
	ValueTask UpdateStoreAsync(int storeId, StoreDto updatedStoreDto, CancellationToken ct);
	ValueTask DeleteStoreAsync(int storeId, CancellationToken ct);
}