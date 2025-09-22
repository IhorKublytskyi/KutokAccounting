using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IStoreBuilder
{
	IQueryable<Store> GetQuery(IQueryable<Store> allStoresQuery, SearchParameters? searchParameters = null);
}