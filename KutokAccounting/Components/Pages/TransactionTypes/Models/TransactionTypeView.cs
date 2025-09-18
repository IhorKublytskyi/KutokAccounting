namespace KutokAccounting.Components.Pages.TransactionTypes.Models;

public sealed record TransactionTypeView
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required bool IsPositiveValue { get; set; }
    public string Label => IsPositiveValue ? "Прибуток" : "Витрата";
}