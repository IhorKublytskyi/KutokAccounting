using KutokAccounting.Services.Stores.Abstractions;
using KutokAccounting.Services.Stores.Dtos;
using KutokAccounting.Services.Stores.Extensions;
using KutokAccounting.Services.Stores.Models;
using KutokAccounting.Services.Vendors.Validators;

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

	public async ValueTask<List<StoreDto>> GetStoresPageAsync(Page page, CancellationToken ct, SearchParameters? searchParameters = null)
	{
		ct.ThrowIfCancellationRequested();
		
		var stores = await _repository.GetFilteredPageOfStoresAsync(page, ct, searchParameters);
		return stores.Select(x => x.ToDto()).ToList();
	}

	public async ValueTask<int> GetAllStoresCountAsync(CancellationToken ct)
	{
		ct.ThrowIfCancellationRequested();

		return await _repository.GetStoresCountAsync();
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