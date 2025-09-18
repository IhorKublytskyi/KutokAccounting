using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IStoreFilter
{
	IQueryable<Store> GetFilteredQuery(IQueryable<Store> allStores, SearchParameters searchParameters);
}