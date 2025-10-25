namespace KutokAccounting.DataProvider;

public static class KutokConfigurations
{
	public const string WriteOperationsSemaphore = "WriteOperationsSemaphore";

	public static readonly string ConnectionString =
		$"Data Source={Path.Combine(AppContext.BaseDirectory, "KutokData.db")}";
}