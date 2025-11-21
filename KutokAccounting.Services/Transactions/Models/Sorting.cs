namespace KutokAccounting.Services.Transactions.Models;

public sealed record Sorting
{
	public string? SortBy { get; set; }
	public bool Descending { get; set; }
}