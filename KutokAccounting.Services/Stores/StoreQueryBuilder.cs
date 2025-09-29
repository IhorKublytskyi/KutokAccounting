using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Helpers;
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
			var utcDateTimeSearched = searchParameters.SetupDate.Value.ToUniversalTime();
			var dateTimeRange = DateTimeHelper.GetHourRange(utcDateTimeSearched);
			
			filteredQuery = filteredQuery.Where(s => 
				s.SetupDate >= dateTimeRange.StartOfRange && s.SetupDate <= dateTimeRange.EndOfRange);
		}
		if (searchParameters.IsOpened != null)
		{
			filteredQuery = filteredQuery.Where(s => s.IsOpened == searchParameters.IsOpened);
		}
		return filteredQuery;
	}
}
