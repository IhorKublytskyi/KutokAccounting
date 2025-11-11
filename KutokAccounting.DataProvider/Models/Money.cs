using System.Globalization;

namespace KutokAccounting.DataProvider.Models;

public readonly record struct Money
{
	public decimal Value { get; }

	public Money(decimal value)
	{
		Value = value;
	}

	public static Money ConvertFromStringToMoney(string value)
	{
		if (value == null)
		{
			throw new ArgumentNullException(nameof(value));
		}

		string replacedValue = value.Replace('.', ',');

		if (decimal.TryParse(replacedValue, out decimal result))
		{
			return new Money(result);
		}

		throw new FormatException("Invalid money format.");
	}

	public long GetValueInCoins()
	{
		return (long) (Value * 100);
	}

	public override string ToString()
	{
		return Value.ToString("C2", CultureInfo.GetCultureInfo("uk-UA"));
	}
}