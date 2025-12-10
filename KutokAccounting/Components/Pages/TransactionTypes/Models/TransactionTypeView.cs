namespace KutokAccounting.Components.Pages.TransactionTypes.Models;

public sealed record TransactionTypeView
{
	public required int Id { get; set; }
	public required string Name { get; set; }
	public required bool IsIncome { get; set; }
	public string Label => IsIncome ? TransactionTypeFiltersConstants.Income : TransactionTypeFiltersConstants.Expense;

	public override string ToString()
	{
		return $"{Name} ({Label})";
	}
}