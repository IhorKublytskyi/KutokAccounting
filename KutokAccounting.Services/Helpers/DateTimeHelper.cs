using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Helpers;

public class DateTimeHelper
{
	public static DateTimeRange GetHourRange(DateTime dateTime)
	{
		DateOnly today = DateOnly.FromDateTime(dateTime);
		DateTime start = today.ToDateTime(TimeOnly.MinValue);
		DateTime end = today.ToDateTime(TimeOnly.MaxValue);

		return new DateTimeRange(start, end);
	}
}