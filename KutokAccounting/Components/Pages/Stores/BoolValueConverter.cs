namespace KutokAccounting.Components.Pages.Stores;

public static class BoolValueConverter
{
	public static string ConvertToString(bool value)
	{
		return value ? "Так" : "Ні";
	}
}