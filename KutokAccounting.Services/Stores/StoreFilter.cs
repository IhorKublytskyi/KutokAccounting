using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Abstractions;
using KutokAccounting.Services.Stores.Models;
using Microsoft.EntityFrameworkCore;

namespace KutokAccounting.Services.Stores;

public class StoreFilter : IStoreFilter
{ 
	public IQueryable<Store> GetFilteredQuery(IQueryable<Store> allStores, SearchParameters searchParameters)
	{
		var query = allStores;
		if (searchParameters.IsOpened is not null)
		{
			query = query.Where(s => s.IsOpened == searchParameters.IsOpened);
		}

		if (searchParameters.Address is not null)
		{
			query = query.Where(s => EF.Functions.Like(s.Address, $"%{searchParameters.Address}%"));
		}
		
		if (searchParameters.SetupDate is not null)
		{
			query = query.Where(s => EF.Functions.Like(s.SetupDate.ToString(), $"%{searchParameters.SetupDate}%"));
		}

		if (searchParameters.Name is not null)
		{
			query = query.Where(s => EF.Functions.Like(s.Name, $"%{searchParameters.Name}%"));
		}
		return query;
	}
}