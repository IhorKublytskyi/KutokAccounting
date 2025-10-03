using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IQueryBuilder
{
	IQueryable<Store> FilterStoresByParametersQuery(IQueryable<Store> allStoresQuery, StoreSearchParameters? searchParameters);
}