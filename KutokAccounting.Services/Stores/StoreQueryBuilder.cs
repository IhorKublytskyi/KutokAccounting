using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Abstractions;
using KutokAccounting.Services.Stores.Models;
using Microsoft.EntityFrameworkCore;

namespace KutokAccounting.Services.Stores;

public class StoreQueryBuilder : IStoreBuilder
{
	public IQueryable<Store> GetQuery(IQueryable<Store> allStoresQuery, SearchParameters? searchParameters = null)
	{
		if (searchParameters is null)
		{
			return allStoresQuery;
		}
		
		return allStoresQuery.Where(s => 
			EF.Functions.Like(s.Address, $"%{searchParameters.Address}%") ||
			EF.Functions.Like(s.Name, $"%{searchParameters.Name}%"));
	}
}