using FluentValidation;
using FluentValidation.Results;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Abstractions;
using KutokAccounting.Services.Stores.Dtos;
using KutokAccounting.Services.Stores.Extensions;
using KutokAccounting.Services.Stores.Models;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Services.Stores;

public class StoresService : IStoresService
{
	private readonly IStoresRepository _repository;
	private readonly IValidator<StoreDto> _storeDtoValidator;
	private readonly ILogger<StoresService> _logger;
	private readonly IValidator<Pagination> _paginationValidator;

	public StoresService(IStoresRepository repository,
		IValidator<StoreDto> storeDtoValidator,
		ILogger<StoresService> logger,
		IValidator<Pagination> paginationValidator)
	{
		_repository = repository;
		_storeDtoValidator = storeDtoValidator;
		_logger = logger;
		_paginationValidator = paginationValidator;
	}

	public async ValueTask CreateAsync(StoreDto storeDto, CancellationToken ct)
	{
		ct.ThrowIfCancellationRequested();
		ValidationResult? validationResult = await _storeDtoValidator.ValidateAsync(storeDto, ct);

		if (validationResult.IsValid is false)
		{
			_logger.LogError("Store validation failed. Errors: {Errors}", validationResult.Errors);
		}

		Store storeModel = storeDto.FromDtoToModel();
		await _repository.CreateStoreAsync(storeModel, ct);

		_logger.LogInformation(
			"A new store with following properties was created. Name: {storeName}, Address: {storeAddress}, Is opened: {isOpened}, Setup date: {setUpDate}"
			, storeDto.Name, storeDto.Address, storeDto.IsOpened.ToString(), storeDto.SetupDate.ToString("mm/dd/yyyy"));
	}

	public async ValueTask<PagedResult<StoreDto>> GetPageAsync(StoreQueryParameters queryParameters,
		CancellationToken ct)
	{
		ct.ThrowIfCancellationRequested();
		ValidationResult? validationResult = await _paginationValidator.ValidateAsync(queryParameters.Pagination, ct);

		if (validationResult.IsValid is false)
		{
			_logger.LogError("Store validation failed. Errors: {Errors}", validationResult.Errors);
		}

		PagedResult<Store> storesPagedResult =
			await _repository.GetFilteredPageOfStoresAsync(queryParameters, ct);

		_logger.LogInformation("Pages of stores were fetched");

		return new PagedResult<StoreDto>
		{
			Count = storesPagedResult.Count,
			Items = storesPagedResult.Items.Select(x => x.ModelToDto())
		};
	}

	public async ValueTask UpdateAsync(int storeId,
		StoreDto updatedStoreDto,
		CancellationToken ct)
	{
		ct.ThrowIfCancellationRequested();

		ValidationResult? validationResult = await _storeDtoValidator.ValidateAsync(updatedStoreDto, ct);

		if (validationResult.IsValid is false)
		{
			_logger.LogError("Failed to update store with Name: {StoreName}: {Errors}", updatedStoreDto.Name,
				validationResult.Errors);

			throw new ArgumentException("Invalid store data");
		}

		Store updatedStoreModel = updatedStoreDto.FromDtoToModel();
		await _repository.UpdateStoreAsync(storeId, updatedStoreModel, ct);

		_logger.LogInformation("Store with id: {StoreId} was updated", storeId);
	}

	public async ValueTask DeleteAsync(int storeId, CancellationToken ct)
	{
		ct.ThrowIfCancellationRequested();

		await _repository.DeleteStoreAsync(storeId, ct);
		_logger.LogInformation("Store with id {storeId} was deleted", storeId);
	}
}