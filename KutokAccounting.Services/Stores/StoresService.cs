using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Abstractions;
using KutokAccounting.Services.Stores.Dtos;
using KutokAccounting.Services.Stores.Extensions;
using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Stores;

public class StoresService : IStoresService
{
	private readonly IStoresRepository _repository;
	private readonly StoreDtoValidator _storeDtoValidator;
	public StoresService(IStoresRepository repository,
		StoreDtoValidator storeDtoValidator)
	{
		_repository = repository;
		_storeDtoValidator = storeDtoValidator;
	}
	
	public async ValueTask CreateStoreAsync(StoreDto storeDto, CancellationToken ct)
	{
		ct.ThrowIfCancellationRequested();

		var storeModel = storeDto.FromDto();
		await _repository.CreateStoreAsync(storeModel, ct);
	}

	public async ValueTask<PagedResult<StoreDto>> GetStoresPageAsync(Pagination pagination, SearchParameters? searchParameters, CancellationToken ct)
	{
		ct.ThrowIfCancellationRequested();
		
		PagedResult<Store> storesPagedResult = await _repository.GetFilteredPageOfStoresAsync(pagination, searchParameters, ct);
		return new PagedResult<StoreDto>()
		{
			Count = storesPagedResult.Count,
			Items = storesPagedResult.Items.Select(x => x.ToDto())
		};
	}
	public async ValueTask UpdateStoreAsync(int storeId, StoreDto updatedStoreDto, CancellationToken ct)
	{
		ct.ThrowIfCancellationRequested();
		
		var validationResult = await _storeDtoValidator.ValidateAsync(updatedStoreDto, ct);
		if (validationResult.IsValid is false)
		{
			throw new ArgumentException("Invalid store data");
		}
		var updatedStoreModel = updatedStoreDto.FromDto();
		await _repository.UpdateStoreAsync(storeId, updatedStoreModel, ct);
	}
	
	public async ValueTask DeleteStoreAsync(int storeId, CancellationToken ct)
	{
        ct.ThrowIfCancellationRequested();
		
		if (storeId < 0)
		{
			throw new ArgumentException("Invalid store id");
		}
		
		await _repository.DeleteStoreAsync(storeId, ct);
	}
}