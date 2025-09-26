using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Abstractions;
using KutokAccounting.Services.Stores.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KutokAccounting.Services.Stores;

public class StoreQueryBuilder : IQueryBuilder
{
	public IQueryable<Store> GetQuery(IQueryable<Store> allStoresQuery, StoreSearchParameters? searchParameters)
	{
		if (searchParameters is null)
		{
			return allStoresQuery;
		}
		var filteredQuery = allStoresQuery;
		if (searchParameters.Name != null)
		{
			filteredQuery = filteredQuery.Where(s => EF.Functions.Like(s.Name, $"%{searchParameters.Name}%"));
		}
		
		if (searchParameters.Address != null)
		{
			filteredQuery = filteredQuery.Where(s => EF.Functions.Like(s.Address, $"%{searchParameters.Address}%"));
		}
		if (searchParameters.SetupDate != null)
		{
			var setUpDateInString = searchParameters.SetupDate?.Date.ToUniversalTime();
			filteredQuery = filteredQuery.Where(s => EF.Functions.Like(s.SetupDate.ToString(), $"%{setUpDateInString}%"));
		}
		if (searchParameters.IsOpened != null)
		{
			filteredQuery = filteredQuery.Where(s => EF.Functions.Like(s.IsOpened.ToString(), $"%{searchParameters.IsOpened.ToString()}%"));
		}
		return filteredQuery;
			
		// return allStoresQuery.Where(s =>
		// 	(s.Address != null && EF.Functions.Like(s.Address, $"%{searchParameters.Address}%")) ||
		// 	(searchParameters.Name != null && EF.Functions.Like(s.Address, $"%{searchParameters.Address}%")) ||
		// 	EF.Functions.Like(s.Name, $"%{searchParameters.Name}%") ||
		// 	EF.Functions.Like(s.SetupDate.ToString(), $"%{setUpDateInString}%") ||
		// 	EF.Functions.Like(s.IsOpened.ToString(), $"%{searchParameters.IsOpened.ToString()}%")
		//  );	

	}
}
