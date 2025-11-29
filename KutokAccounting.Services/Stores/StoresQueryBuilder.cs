using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Helpers;
using KutokAccounting.Services.Stores.Models;
using Microsoft.EntityFrameworkCore;

namespace KutokAccounting.Services.Stores;

public class StoresQueryBuilder
{
	private IQueryable<Store> _query;

	public StoresQueryBuilder(KutokDbContext dbContext)
	{
		_query = dbContext.Stores.AsNoTracking();
	}

	public StoresQueryBuilder SearchId(int? id)
	{
		if (id is not null)
		{
			_query = _query.Where(s => s.Id == id);
		}

		return this;
	}

	public StoresQueryBuilder SearchName(string? name)
	{
		if (string.IsNullOrEmpty(name) is false)
		{
			_query = _query.Where(s => EF.Functions.Like(s.Name, $"%{name}%"));
		}

		return this;
	}

	public StoresQueryBuilder SearchAddress(string? address)
	{
		if (string.IsNullOrEmpty(address) is false)
		{
			_query = _query.Where(s => EF.Functions.Like(s.Address, $"%{address}%"));
		}

		return this;
	}

	public StoresQueryBuilder SearchSetupDate(DateTime? setupDate)
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

	public StoresQueryBuilder SearchOpened(bool? isOpened)
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