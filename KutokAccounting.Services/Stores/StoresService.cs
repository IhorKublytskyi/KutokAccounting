using KutokAccounting.Services.Stores.Abstractions;
using KutokAccounting.Services.Stores.Dtos;
using KutokAccounting.Services.Stores.Extensions;

namespace KutokAccounting.Services.Stores;

public class StoresService : IStoresService
{
	private readonly IStoresRepository _repository;
	public StoresService(IStoresRepository repository)
	{
		_repository = repository;
	}

	public async ValueTask CreateStoreAsync(StoreDto storeDto, CancellationToken ct)
	{
		var storeModel = storeDto.FromDto();
		await _repository.CreateStoreAsync(storeModel, ct);
	}
	
	public IQueryable<StoreDto> GetStoresPageAsync(int pageSize, int pageNumber)
	{
		return _repository.GetStoresPage(pageSize, pageNumber).Select(x => x.ToDto());
	}

	public async ValueTask<int> GetAllStoresCountAsync()
	{
		return await _repository.GetStoresCountAsync();
	}

	public async ValueTask UpdateStoreAsync(int storeId, StoreDto updatedStoreDto, CancellationToken ct)
	{
		
		var updatedStoreModel = updatedStoreDto.FromDto();
		await _repository.UpdateStoreAsync(storeId, updatedStoreModel, ct);
	}
	
	public async ValueTask DeleteStoreAsync(int storeId, CancellationToken ct)
	{
		if (storeId < 0)
		{
			throw new ArgumentException("Invalid store id");
		}
		
		await _repository.DeleteStoreAsync(storeId, ct);
	}
}