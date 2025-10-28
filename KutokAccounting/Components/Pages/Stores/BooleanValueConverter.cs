namespace KutokAccounting.Components.Pages.Stores;

public static class BooleanValueConverter
{
	public static string ConvertToString(bool value)
	{
		return value ? "Так" : "Ні";
	}
}