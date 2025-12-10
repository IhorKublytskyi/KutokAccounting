using System.Text.RegularExpressions;

namespace KutokAccounting;

public static partial class MoneyFormatRegex
{
	[GeneratedRegex(@"^(0|[1-9]\d*)(\.|,)([1-9]|\d\d)$", RegexOptions.IgnoreCase, "en-US")]
	public static partial Regex MoneyValueRegex();
}