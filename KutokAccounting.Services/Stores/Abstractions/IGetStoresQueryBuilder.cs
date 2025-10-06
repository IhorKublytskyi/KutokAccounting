using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface IGetStoresQueryBuilder
{
	IQueryable<Store> GetStoresByParametersQuery(StoreQueryParameters searchParameters);
}