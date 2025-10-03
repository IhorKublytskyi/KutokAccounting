using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Helpers;

public class DateTimeHelper
{
	public static DateTimeRange GetDayStartEndRange(DateTime dateTime)
	{
		DateOnly date = DateOnly.FromDateTime(dateTime);
		DateTime start = date.ToDateTime(TimeOnly.MinValue);
		DateTime end = date.ToDateTime(TimeOnly.MaxValue);

		return new DateTimeRange(start, end);
	}
}