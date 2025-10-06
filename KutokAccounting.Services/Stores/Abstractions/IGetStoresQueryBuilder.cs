using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IGetStoresQueryBuilder
{
	IGetStoresQueryBuilder EmptyPreviousQuery();
	IGetStoresQueryBuilder SearchName(string? name);
	IGetStoresQueryBuilder SearchAddress(string? address);
	IGetStoresQueryBuilder SearchSetupDate(DateTime? setupDate);
	IGetStoresQueryBuilder SearchOpened(bool? isOpened);
	IQueryable<Store> BuildQuery();
}