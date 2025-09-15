using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Dtos;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IStoresService
{
	Task CreateStoreAsync(StoreDto storeDto, CancellationToken ct);
	IQueryable<StoreDto> GetStoresPageAsync(int pageSize, int pageNumber);
	int GetAllStoresCount();
	Task UpdateStore(int storeId, StoreDto updatedStoreDto, CancellationToken ct);
	ValueTask DeleteStore(int storeId, CancellationToken ct);
}