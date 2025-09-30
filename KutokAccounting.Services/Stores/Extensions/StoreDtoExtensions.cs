using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Dtos;

namespace KutokAccounting.Services.Stores.Extensions;

public static class StoreDtoExtensions
{
	public static Store FromDtoToModel(this StoreDto dto)
	{
		return new Store
		{
			Name = dto.Name,
			IsOpened = dto.IsOpened,
			SetupDate = dto.SetupDate,
			Address = dto.Address,
		};
	}
	//box struct -> class
	public static StoreDto ToDto(this Store store)
	{
		return new()
		{
			Id = store.Id,
			Name = store.Name,
			IsOpened = store.IsOpened,
			SetupDate = store.SetupDate,
			Address = store.Address,
		};
	}
}