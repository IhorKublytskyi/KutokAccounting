using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Dtos;
using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IStoresService
{
	ValueTask CreateAsync(StoreDto storeDto, CancellationToken ct);

	ValueTask<PagedResult<StoreDto>> GetPageAsync(
		StoreQueryParameters queryParameters,
		CancellationToken ct);

	/// <summary>
	///     returns store with requested id
	/// </summary>
	/// <param name="id">lookup id</param>
	/// <param name="ct">CancellationToken</param>
	/// <returns>store with requested id</returns>
	/// <exception cref="ArgumentException">thrown when id does not exist</exception>
	ValueTask<StoreDto> GetByIdAsync(int id, CancellationToken ct);

	ValueTask UpdateAsync(int storeId,
		StoreDto updatedStoreDto,
		CancellationToken ct);

	ValueTask DeleteAsync(int storeId, CancellationToken ct);
}