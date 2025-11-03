using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Helpers;
using KutokAccounting.Services.Stores.Models;
using Microsoft.EntityFrameworkCore;

namespace KutokAccounting.Services.Stores;

public class GetStoresQueryBuilder
{
	private IQueryable<Store> _query;

	public GetStoresQueryBuilder(KutokDbContext dbContext)
	{
		_query = dbContext.Stores.AsNoTracking();
	}

	public GetStoresQueryBuilder SearchId(int? id)
	{
		if (id is not null)
		{
			_query = _query.Where(s => s.Id == id);
		}

		return this;
	}
	public GetStoresQueryBuilder SearchName(string? name)
	{
		if (string.IsNullOrEmpty(name) is false)
		{
			_query = _query.Where(s => EF.Functions.Like(s.Name, $"%{name}%"));
		}

		return this;
	}

	public GetStoresQueryBuilder SearchAddress(string? address)
	{
		if (string.IsNullOrEmpty(address) is false)
		{
			_query = _query.Where(s => EF.Functions.Like(s.Address, $"%{address}%"));
		}

		return this;
	}

	public GetStoresQueryBuilder SearchSetupDate(DateTime? setupDate)
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
	
	public GetStoresQueryBuilder SearchOpened(bool? isOpened)
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