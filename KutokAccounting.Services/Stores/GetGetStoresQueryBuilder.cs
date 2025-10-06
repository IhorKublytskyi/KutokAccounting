using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Helpers;
using KutokAccounting.Services.Stores.Abstractions;
using KutokAccounting.Services.Stores.Models;
using Microsoft.EntityFrameworkCore;

namespace KutokAccounting.Services.Stores;

public class GetGetStoresQueryBuilder : IGetStoresQueryBuilder
{
	private IQueryable<Store> _query;
	private readonly KutokDbContext _dbContext;

	public GetGetStoresQueryBuilder(KutokDbContext dbContext)
	{
		_dbContext = dbContext;
		_query = dbContext.Stores.AsNoTracking();
	}

	public IGetStoresQueryBuilder EmptyPreviousQuery()
	{
		_query = _dbContext.Stores.AsNoTracking();
		return this;
	}

	public IGetStoresQueryBuilder SearchName(string? name)
	{
		if (string.IsNullOrEmpty(name) is false)
		{
			_query = _query.Where(s => EF.Functions.Like(s.Name, $"%{name}%"));
		}

		return this;
	}

	public IGetStoresQueryBuilder SearchAddress(string? address)
	{
		if (string.IsNullOrEmpty(address) is false)
		{
			_query = _query.Where(s => EF.Functions.Like(s.Address, $"%{address}%"));
		}

		return this;
	}

	public IGetStoresQueryBuilder SearchSetupDate(DateTime? setupDate)
	{
		if (setupDate is not null)
		{
			DateTime utcDateTimeSearched = setupDate.Value.ToUniversalTime();
			DateTimeRange dateTimeRange = DateTimeHelper.GetDayStartEndRange(utcDateTimeSearched);

			_query = _query.Where(s =>
				s.SetupDate >= dateTimeRange.StartOfRange && s.SetupDate <= dateTimeRange.EndOfRange);
		}
		
		return this;
	}

	public IGetStoresQueryBuilder SearchOpened(bool? isOpened)
	{
		if (isOpened is not null)
		{
			_query = _query.Where(s => s.IsOpened == isOpened);
		}

		return this;
	}

	public IQueryable<Store> BuildQuery()
	{
		return _query;
	}
}