using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Models;
using KutokAccounting.Services.Transactions.Models;

namespace KutokAccounting.Services.Transactions.Interfaces;

public interface ITransactionService
{
	ValueTask<PagedResult<Transaction>> GetAsync(TransactionQueryParameters transactionQueryParameters, CancellationToken cancellationToken);

	ValueTask<CalculationResult> GetCalculation(DateTimeRange? range, CancellationToken cancellationToken);

	ValueTask<Transaction> GetByIdAsync(int id, CancellationToken cancellationToken);
	ValueTask<Transaction> CreateAsync(TransactionDto request, CancellationToken cancellationToken);
	ValueTask UpdateAsync(TransactionDto request, CancellationToken cancellationToken);
	ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
}