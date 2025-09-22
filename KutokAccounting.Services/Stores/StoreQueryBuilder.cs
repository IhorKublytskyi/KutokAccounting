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
		
		if (searchParameters.IsOpened is not null)
		{
			allStoresQuery = allStoresQuery.Where(s => s.IsOpened == searchParameters.IsOpened);
		}

		if (searchParameters.Address is not null)
		{
			allStoresQuery = allStoresQuery.Where(s => EF.Functions.Like(s.Address, $"%{searchParameters.Address}%"));
		}
		
		if (searchParameters.SetupDate is not null)
		{
			allStoresQuery = allStoresQuery.Where(s => EF.Functions.Like(s.SetupDate.ToString(), $"%{searchParameters.SetupDate}%"));
		}

		if (searchParameters.Name is not null)
		{
			allStoresQuery = allStoresQuery.Where(s => EF.Functions.Like(s.Name, $"%{searchParameters.Name}%"));
		}
		return allStoresQuery;
	}
}