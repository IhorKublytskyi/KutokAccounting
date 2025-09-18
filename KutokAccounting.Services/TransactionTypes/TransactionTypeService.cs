using FluentValidation.Results;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.TransactionTypes.Interfaces;
using KutokAccounting.Services.TransactionTypes.Models;
using KutokAccounting.Services.TransactionTypes.Validators;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Services.TransactionTypes;

public sealed class TransactionTypeService : ITransactionTypeService
{
    private readonly ILogger<TransactionTypeService> _logger;
    private readonly TransactionTypeDtoValidator _transactionTypeDtoValidator;
    private readonly TransactionTypeQueryParametersValidator _transactionTypeQueryValidator;
    private readonly ITransactionTypeRepository _repository;
    

    public TransactionTypeService(
        ITransactionTypeRepository repository, 
        ILogger<TransactionTypeService> logger, 
        TransactionTypeDtoValidator transactionTypeDtoValidator, 
        TransactionTypeQueryParametersValidator transactionTypeQueryValidator)
    {
        _repository = repository;
        _logger = logger;
        _transactionTypeDtoValidator = transactionTypeDtoValidator;
        _transactionTypeQueryValidator = transactionTypeQueryValidator;
    }

    public async ValueTask<TransactionType> CreateAsync(TransactionTypeDto request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ValidationResult validationResult = await _transactionTypeDtoValidator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid is false)
        {
            _logger.LogWarning("Transaction type creation request validation failed. Errors: {Errors}", validationResult.Errors);

            throw new ArgumentException(validationResult.ToString());
        }

        _logger.LogInformation("Validation succeeded for vendor {TransactionTypeName}", request.Name);

        TransactionType transactionType = new TransactionType()
        {
            Name = request.Name,
            IsPositiveValue = request.IsPositiveValue
        };
        
        _logger.LogInformation("Saving transaction type to repository. Name: {TransactionTypeName}", transactionType.Name);

        await _repository.CreateAsync(transactionType, cancellationToken);

        _logger.LogInformation("Transaction type {TransactionTypeName} successfully created with ID {TransactionTypeId}", transactionType.Name, transactionType.Id);

        return transactionType;
    }

    public async ValueTask<TransactionType> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Fetching transaction type by ID: {TransactionTypeId}", id);
        
        TransactionType? transactionType = await _repository.GetByIdAsync(id, cancellationToken);

        if (transactionType is null)
        {
            _logger.LogWarning("Transaction type with ID {TransactionTypeId} not found", id);

            throw new Exception("Transaction type not found");
        }

        _logger.LogInformation("Transaction type with ID {TransactionTypeId} retrieved successfully. Name: {TransactionTypeName}", transactionType.Id,
            transactionType.Name);

        return transactionType;
    }

    public async ValueTask<PagedResult<TransactionType>> GetAsync(QueryParameters queryParameters, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ValidationResult validationResult = await _transactionTypeQueryValidator.ValidateAsync(queryParameters, cancellationToken);
        
        if (validationResult.IsValid is false)
        {
            _logger.LogWarning("Query parameters validation failed. Errors: {Errors}", validationResult.Errors);

            throw new ArgumentException(validationResult.ToString());
        }

        PagedResult<TransactionType> transactionTypes = await _repository.GetAsync(queryParameters, cancellationToken);

        if (transactionTypes is { Count: 0 })
        {
            _logger.LogWarning("No transaction types found for query parameters: {@QueryParameters}", queryParameters);

            throw new Exception("Transaction types list is empty");
        }
        
        _logger.LogInformation("Retrieved {Count} transaction types successfully", transactionTypes.Count);

        return transactionTypes;
    }

    public async ValueTask UpdateAsync(TransactionTypeDto request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Updating transaction type with data: {TransactionTypeRequest}", request);

        ValidationResult validationResult = await _transactionTypeDtoValidator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid is false)
        {
            _logger.LogWarning("Transaction type update validation failed: {ValidationErrors}", validationResult.Errors);

            throw new Exception(validationResult.ToString());
        }

        _logger.LogDebug("Validation succeeded for transaction type update: {TransactionTypeName}", request.Name);

        TransactionType transactionType = new TransactionType
        {
            Id = request.Id,
            Name = request.Name,
            IsPositiveValue = request.IsPositiveValue
        };

        await _repository.UpdateAsync(transactionType, cancellationToken);

        _logger.LogInformation("Transaction type {TransactionTypeName} updated successfully", transactionType.Name);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _repository.DeleteAsync(id, cancellationToken);

        _logger.LogInformation("Transaction type with ID {TransactionTypeId} deleted successfully", id);
    }
}