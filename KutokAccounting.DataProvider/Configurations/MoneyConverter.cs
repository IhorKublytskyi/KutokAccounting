using KutokAccounting.DataProvider.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KutokAccounting.DataProvider.Configurations;

public class MoneyConverter : ValueConverter<Money, long>
{
	public MoneyConverter() : base(
		m => m.Value,
		m => new Money(m)
	)
	{
	}
}