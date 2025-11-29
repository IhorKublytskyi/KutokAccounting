namespace KutokAccounting.DataProvider.Models;

public record CalculationResult
{
	public Money Profit { get; set; }
	public Money Income { get; set; }
	public Money Expense { get; set; }
};
