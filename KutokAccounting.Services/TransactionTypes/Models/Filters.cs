namespace KutokAccounting.Services.TransactionTypes.Models;

public sealed record Filters
{
    public string? Name { get; set; }
    public bool? IsPositiveValue { get; set; }
}
