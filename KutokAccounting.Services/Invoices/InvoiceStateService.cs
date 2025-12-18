using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Invoices.Interfaces;
using KutokAccounting.Services.Invoices.Models;
using KutokAccounting.Services.Invoices.Validators;
using KutokAccounting.Services.Transactions.Interfaces;
using KutokAccounting.Services.TransactionTypes.Exceptions;
using KutokAccounting.Services.TransactionTypes.Interfaces;

namespace KutokAccounting.Services.Invoices;

public class InvoiceStateService : IInvoiceStateService
{
	private readonly IInvoiceRepository _invoiceRepository;
	private readonly ITransactionRepository _transactionRepository;
	private readonly ITransactionTypeRepository _transactionTypeRepository;

	public InvoiceStateService(IInvoiceRepository invoiceRepository,
		ITransactionRepository transactionRepository,
		ITransactionTypeRepository transactionTypeRepository)
	{
		_invoiceRepository = invoiceRepository;
		_transactionRepository = transactionRepository;
		_transactionTypeRepository = transactionTypeRepository;
	}

	public async ValueTask<Invoice> OpenInvoiceAsync(InvoiceDto request, CancellationToken cancellationToken)
	{
		Invoice invoice = new()
		{
			Number = request.Number,
			StoreId = request.StoreId,
			VendorId = request.VendorId,
			StatusHistory =
			[
				new InvoiceStatus
				{
					CreatedAt = DateTime.Now,
					State = State.Active
				}
			],
			CreatedAt = DateTime.Now
		};

		await _invoiceRepository.CreateAsync(invoice, cancellationToken);

		TransactionType? transactionType =
			await _transactionTypeRepository.GetByCodeAsync(KutokConfigurations.OpenInvoiceTransactionTypeCode,
				cancellationToken);

		if (transactionType is null)
		{
			throw new NotFoundException("The service transaction type was not found.");
		}

		Transaction transaction = new()
		{
			Name = "ВН" + request.Number,
			Description = "Відкриття накладної №" + request.Number,
			Money = request.Money,
			CreatedAt = DateTime.Now,
			StoreId = request.StoreId,
			InvoiceId = invoice.Id,
			TransactionTypeId = transactionType.Id
		};

		await _transactionRepository.CreateAsync(transaction, cancellationToken);

		return invoice;
	}

	public async ValueTask CloseInvoiceAsync(CloseInvoiceDto request, CancellationToken cancellationToken)
	{
		Invoice invoice = new()
		{
			Id = request.Id,
			Number = request.Number
		};

		await _invoiceRepository.CloseAsync(invoice, cancellationToken);

		TransactionType? transactionType =
			await _transactionTypeRepository.GetByCodeAsync(KutokConfigurations.CloseInvoiceTransactionTypeCode,
				cancellationToken);

		if (transactionType is null)
		{
			throw new NotFoundException("The service transaction type was not found.");
		}

		Transaction transaction = new()
		{
			Name = "ЗН" + request.Number,
			Description = "Закриття накладної №" + request.Number,
			Money = request.Money,
			CreatedAt = DateTime.Now,
			StoreId = request.StoreId,
			InvoiceId = invoice.Id,
			TransactionTypeId = transactionType.Id
		};

		await _transactionRepository.CreateAsync(transaction, cancellationToken);
	}
}