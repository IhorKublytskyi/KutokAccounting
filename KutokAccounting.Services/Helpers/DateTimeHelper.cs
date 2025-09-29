using KutokAccounting.Services.Stores.Models;

namespace KutokAccounting.Services.Helpers;

public class DateTimeHelper
{
	public static DateTimeRange GetHourRange(DateTime dateTime)
	{
		var hoursOffset = TimeSpan.FromHours(23) + TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(59);

		var startOfRange = dateTime
			.Subtract(TimeSpan.FromHours(dateTime.Hour))
			.Subtract(TimeSpan.FromMinutes(dateTime.Minute))
			.Subtract(TimeSpan.FromSeconds(dateTime.Second));
		
		var endOfRange = startOfRange + hoursOffset;
		return new DateTimeRange(startOfRange, endOfRange);
	}
}