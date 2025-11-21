using FluentValidation;
using FluentValidation.Results;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Models;
using KutokAccounting.Services.Transactions.Interfaces;
using KutokAccounting.Services.Transactions.Models;
using KutokAccounting.Services.TransactionTypes.Exceptions;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Services.Transactions;

public sealed class TransactionService : ITransactionService
{
	private readonly ITransactionRepository _repository;
	private readonly ILogger<Transaction> _logger;
	private readonly IValidator<TransactionDto> _transactionDtoValidator;
	private readonly IValidator<TransactionQueryParameters> _transactionQueryValidator;

	public TransactionService(
		ITransactionRepository repository, 
		ILogger<Transaction> logger, 
		IValidator<TransactionDto> transactionDtoValidator, 
		IValidator<TransactionQueryParameters> transactionQueryValidator)
	{
		_repository = repository;
		_logger = logger;
		_transactionDtoValidator = transactionDtoValidator;
		_transactionQueryValidator = transactionQueryValidator;
	}
	public async ValueTask<Transaction> CreateAsync(TransactionDto request, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		ValidationResult validationResult = await _transactionDtoValidator.ValidateAsync(request, cancellationToken);

		if (validationResult.IsValid is false)
		{
			_logger.LogWarning("Transaction creation request validation failed. Errors: {Errors}",
				validationResult.Errors);

			throw new ArgumentException(validationResult.ToString());
		}
		
		_logger.LogInformation("Validation succeeded for transaction {TransactionName}", request.Name);

		Transaction transaction = new()
		{
			Name = request.Name,
			Description = request.Description,
			Money = new Money(request.Value),
			CreatedAt = DateTime.Now,
			StoreId = 2,
			TransactionTypeId = request.TransactionTypeId,
			InvoiceId = 1
		};
		
		_logger.LogInformation("Saving transaction to repository. Name: {TransactionName}",
			transaction.Name);

		await _repository.CreateAsync(transaction, cancellationToken);
		
		_logger.LogInformation(
			"Transaction  {TransactionName} successfully created with ID {TransactionId}",
			transaction.Name, transaction.Id);

		return transaction;
	}
	
	public async ValueTask<PagedResult<Transaction>> GetAsync(TransactionQueryParameters transactionQueryParameters, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		ValidationResult validationResult =
			await _transactionQueryValidator.ValidateAsync(transactionQueryParameters, cancellationToken);

		if (validationResult.IsValid is false)
		{
			_logger.LogWarning("Query parameters for transaction validation failed. Errors: {Errors}", validationResult.Errors);

			throw new ArgumentException(validationResult.ToString());
		}

		PagedResult<Transaction> transactions =
			await _repository.GetAsync(transactionQueryParameters, cancellationToken);

		if (transactions is {Count: 0})
		{
			_logger.LogWarning("No transactions found for query parameters: {@QueryParameters}",
				transactionQueryParameters);

			throw new Exception("Transaction list is empty");
		}

		_logger.LogInformation("Retrieved {Count} transactions successfully", transactions.Count);

		return transactions;
	}

	public async ValueTask<CalculationResult> GetCalculation(DateTimeRange? range, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		TransactionQueryParameters parameters = new(
			new Filters 
			{
				Range = range 
			},
			null,
			null,
			new Pagination 
			{ 
				Page = 1, 
				PageSize = int.MaxValue 
			});

		PagedResult<Transaction> transactions = await _repository.GetAsync(parameters, cancellationToken);

		long income = transactions.Items
			.Where(t => t.TransactionType?.IsIncome is true)
			.Select(t => t.Money.Value)
			.Sum();

		long expense = transactions.Items
			.Where(t => t.TransactionType?.IsIncome is false)
			.Select(t => -t.Money.Value)
			.Sum();

		return new CalculationResult
		{
			Income = new Money(income),
			Expense = new Money(expense),
			Profit = new Money(income + expense)
		};
    }
	
	public async ValueTask<Transaction> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		
		_logger.LogInformation("Fetching transaction by ID: {TransactionId}", id);

		Transaction? transaction = await _repository.GetByIdAsync(id, cancellationToken);

		if (transaction is null)
		{
			_logger.LogWarning("Transaction with ID {TransactionId} not found", id);

			throw new NotFoundException("Transaction not found");
		}

		_logger.LogInformation(
			"Transaction with ID {TransactionId} retrieved successfully. Name: {TransactionName}",
			transaction.Id,
			transaction.Name);

		return transaction;
	}

	public async ValueTask UpdateAsync(TransactionDto request, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		_logger.LogInformation("Updating transaction with data: {TransactionRequest}", request);

		ValidationResult validationResult =
			await _transactionDtoValidator.ValidateAsync(request, cancellationToken);

		if (validationResult.IsValid is false)
		{
			_logger.LogWarning("Transaction update validation failed: {ValidationErrors}",
				validationResult.Errors);

			throw new Exception(validationResult.ToString());
		}

		_logger.LogDebug("Validation succeeded for transaction update: {TransactionName}", request.Name);

		Transaction transaction = new()
		{
			Id = request.Id,
			Name = request.Name,
			Description = request.Description,
			Money = new Money(request.Value),
			TransactionTypeId = request.TransactionTypeId,
			InvoiceId = request.InvoiceId
		};

		await _repository.UpdateAsync(transaction, cancellationToken);

		_logger.LogInformation("Transaction {TransactionName} updated successfully", transaction.Name);
	}

	public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await _repository.DeleteAsync(id, cancellationToken);

		_logger.LogInformation("Transaction with ID {TransactionId} deleted successfully", id);
	}
}