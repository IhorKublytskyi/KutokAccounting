namespace KutokAccounting.DataProvider.Models;

public class TransactionType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsPositiveValue { get; set; }
    public IEnumerable<Transaction> Transactions { get; set; } = null!;
}