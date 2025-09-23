namespace KutokAccounting.Services.TransactionTypes.Models;

public sealed record TransactionTypeDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsIncome { get; set; }
};