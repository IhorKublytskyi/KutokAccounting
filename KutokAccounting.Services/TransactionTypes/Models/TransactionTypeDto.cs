namespace KutokAccounting.Services.TransactionTypes.Models;

public sealed record TransactionTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsPositiveValue { get; set; }
};