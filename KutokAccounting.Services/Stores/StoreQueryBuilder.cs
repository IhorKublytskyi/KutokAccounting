using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Helpers;
using KutokAccounting.Services.Stores.Abstractions;
using KutokAccounting.Services.Stores.Models;
using Microsoft.EntityFrameworkCore;

namespace KutokAccounting.Services.Stores;

public class StoreQueryBuilder : IQueryBuilder
{
	public IQueryable<Store> FilterStoresByParametersQuery(IQueryable<Store> allStoresQuery,
		StoreQueryParameters? searchParameters)
	{
		if (searchParameters is null)
		{
			return allStoresQuery;
		}

		IQueryable<Store> filteredQuery = allStoresQuery;

		if (string.IsNullOrEmpty(searchParameters.Name) is false)
		{
			filteredQuery = filteredQuery.Where(s => EF.Functions.Like(s.Name, $"%{searchParameters.Name}%"));
		}

		if (string.IsNullOrEmpty(searchParameters.Address) is false)
		{
			filteredQuery = filteredQuery.Where(s => EF.Functions.Like(s.Address, $"%{searchParameters.Address}%"));
		}

		if (searchParameters.SetupDate != null)
		{
			DateTime utcDateTimeSearched = searchParameters.SetupDate.Value.ToUniversalTime();
			DateTimeRange dateTimeRange = DateTimeHelper.GetDayStartEndRange(utcDateTimeSearched);

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