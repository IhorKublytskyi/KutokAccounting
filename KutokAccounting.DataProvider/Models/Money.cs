using System.Globalization;

namespace KutokAccounting.DataProvider.Models;

public readonly record struct Money
{
	public long Value { get; }

	public Money(long value)
	{
		Value = value;
	}

	public static bool operator >(Money left, Money right)
	{
		return left.Value > right.Value;
	}

	public static bool operator <(Money left, Money right)
	{
		return left.Value < right.Value;
	}

	public static Money ConvertFromStringToMoney(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ArgumentNullException(nameof(value));
		}

		string formattedString = value.Replace('.', ',');

		if (decimal.TryParse(formattedString, out decimal result))
		{
			return new Money((long) (result * 100));
		}

		throw new FormatException("Invalid money format.");
	}

	public decimal GetValueInCoins()
	{
		return decimal.Divide(Value, 100);
	}

	public override string ToString()
	{
		decimal decimalValue = decimal.Divide(Value, 100);

		return decimalValue.ToString("C2", CultureInfo.GetCultureInfo("uk-UA"));
	}

	public string ToString(string format)
	{
		return GetValueInCoins().ToString(format);
	}
}