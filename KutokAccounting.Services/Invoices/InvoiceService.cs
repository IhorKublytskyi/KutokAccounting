using FluentValidation;
using FluentValidation.Results;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Invoices.Interfaces;
using KutokAccounting.Services.Invoices.Models;
using KutokAccounting.Services.Invoices.Validators;
using KutokAccounting.Services.TransactionTypes.Exceptions;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Services.Invoices;

public class InvoiceService : IInvoiceService
{
	private readonly IInvoiceStateService _invoiceStateService;
	private readonly IInvoiceRepository _repository;
	private readonly ILogger<InvoiceService> _logger;
	private readonly IValidator<InvoiceDto> _invoiceDtoValidator;
	private readonly IValidator<InvoiceQueryParameters> _invoiceQueryValidator;
	private readonly IValidator<CloseInvoiceDto> _closeInvoiceDtoValidator;

	public InvoiceService(
		IInvoiceStateService invoiceStateService,
		IInvoiceRepository repository,
		ILogger<InvoiceService> logger,
		IValidator<InvoiceDto> invoiceDtoValidator,
		IValidator<InvoiceQueryParameters> invoiceQueryValidator,
		IValidator<CloseInvoiceDto> closeInvoiceDtoValidator)
	{
		_invoiceStateService = invoiceStateService;
		_repository = repository;
		_logger = logger;
		_invoiceDtoValidator = invoiceDtoValidator;
		_invoiceQueryValidator = invoiceQueryValidator;
		_closeInvoiceDtoValidator = closeInvoiceDtoValidator;
	}

	public async ValueTask<PagedResult<Invoice>> GetAsync(InvoiceQueryParameters parameters,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		ValidationResult validationResult = await _invoiceQueryValidator.ValidateAsync(parameters, cancellationToken);

		if (validationResult.IsValid is false)
		{
			_logger.LogWarning("Query parameters for invoice validation failed. Errors: {Errors}",
				validationResult.Errors);

			throw new ArgumentException(validationResult.ToString());
		}

		PagedResult<Invoice> invoices = await _repository.GetAsync(parameters, cancellationToken);

		if (invoices is {Count: 0})
		{
			_logger.LogWarning("No invoices found for query parameters: {@Parameters}",
				parameters);

			throw new Exception("Invoice list is empty");
		}

		_logger.LogInformation("Retrieved {Count} invoices successfully", invoices.Count);

		return invoices;
	}

	public async ValueTask<Invoice> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		_logger.LogInformation("Fetching invoice by ID: {InvoiceId}", id);

		Invoice? invoice = await _repository.GetByIdAsync(id, cancellationToken);

		if (invoice is null)
		{
			_logger.LogWarning("Invoice with ID {InvoiceId} not found", id);

			throw new NotFoundException("Invoice not found");
		}

		_logger.LogInformation(
			"Invoice with ID {InvoiceId} retrieved successfully. Number: {InvoiceNumber}",
			invoice.Id,
			invoice.Number);

		return invoice;
	}

	public async ValueTask<Invoice> CreateAsync(InvoiceDto request, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		ValidationResult validationResult =
			await _invoiceDtoValidator.ValidateAsync(request, cancellationToken);

		if (validationResult.IsValid is false)
		{
			_logger.LogWarning("Invoice creation request validation failed. Errors: {Errors}",
				validationResult.Errors);

			throw new ArgumentException(validationResult.ToString());
		}

		Invoice invoice = await _invoiceStateService.OpenInvoiceAsync(request, cancellationToken);

		return invoice;
	}

	public async ValueTask UpdateAsync(InvoiceDto request, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		_logger.LogInformation("Updating invoice with data: {InvoiceRequest}", request);

		ValidationResult validationResult =
			await _invoiceDtoValidator.ValidateAsync(request, cancellationToken);

		if (validationResult.IsValid is false)
		{
			_logger.LogWarning("Invoice update validation failed: {ValidationErrors}",
				validationResult.Errors);

			throw new Exception(validationResult.ToString());
		}

		_logger.LogDebug("Validation succeeded for invoice update: {InvoiceNumber}", request.Number);

		Invoice invoice = new()
		{
			Number = request.Number,
			VendorId = request.VendorId,
			StatusHistory = null
		};

		await _repository.UpdateAsync(invoice, cancellationToken);

		_logger.LogInformation("Invoice {InvoiceNumber} updated successfully", invoice.Number);
	}

	public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await _repository.DeleteAsync(id, cancellationToken);

		_logger.LogInformation("Invoice with ID {InvoiceId} deleted successfully", id);
	}

	public async ValueTask CloseAsync(CloseInvoiceDto request, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		ValidationResult validationResult = await _closeInvoiceDtoValidator.ValidateAsync(request, cancellationToken);

		if (validationResult.IsValid is false)
		{
			_logger.LogWarning("Invoice closing validation failed: {ValidationErrors}",
				validationResult.Errors);

			throw new Exception(validationResult.ToString());
		}

		await _invoiceStateService.CloseInvoiceAsync(request, cancellationToken);
	}
}