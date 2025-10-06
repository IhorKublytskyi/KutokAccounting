using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Helpers;
using KutokAccounting.Services.Stores.Abstractions;
using KutokAccounting.Services.Stores.Models;
using Microsoft.EntityFrameworkCore;

namespace KutokAccounting.Services.Stores;

public class GetStoresQueryBuilder : IGetStoresQueryBuilder
{
	private readonly KutokDbContext _dbContext;
	public GetStoresQueryBuilder(KutokDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	public IQueryable<Store> GetStoresByParametersQuery(StoreQueryParameters searchParameters)
	{
		IQueryable<Store> query = _dbContext.Stores.AsNoTracking();

		if (string.IsNullOrEmpty(searchParameters.Name) is false)
		{
			query = query.Where(s => EF.Functions.Like(s.Name, $"%{searchParameters.Name}%"));
		}

		if (string.IsNullOrEmpty(searchParameters.Address) is false)
		{
			query = query.Where(s => EF.Functions.Like(s.Address, $"%{searchParameters.Address}%"));
		}

		if (searchParameters.SetupDate != null)
		{
			DateTime utcDateTimeSearched = searchParameters.SetupDate.Value.ToUniversalTime();
			DateTimeRange dateTimeRange = DateTimeHelper.GetDayStartEndRange(utcDateTimeSearched);

			query = query.Where(s =>
				s.SetupDate >= dateTimeRange.StartOfRange && s.SetupDate <= dateTimeRange.EndOfRange);
		}

		if (searchParameters.IsOpened != null)
		{
			query = query.Where(s => s.IsOpened == searchParameters.IsOpened);
		}

		return query;
	}
}