using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Invoices.Interfaces;
using KutokAccounting.Services.Invoices.Models;
using KutokAccounting.Services.Transactions.Interfaces;
using KutokAccounting.Services.TransactionTypes.Interfaces;

namespace KutokAccounting.Services;

public class InvoiceTransactionHelper : IInvoiceTransactionHelper
{
	private readonly IInvoiceRepository _invoiceRepository;
	private readonly ITransactionRepository _transactionRepository;
	private readonly ITransactionTypeRepository _transactionTypeRepository;

	public InvoiceTransactionHelper(IInvoiceRepository invoiceRepository,
		ITransactionRepository transactionRepository,
		ITransactionTypeRepository transactionTypeRepository)
	{
		_invoiceRepository = invoiceRepository;
		_transactionRepository = transactionRepository;
		_transactionTypeRepository = transactionTypeRepository;
	}

	public async ValueTask<Invoice> OpenAsync(InvoiceDto request, CancellationToken cancellationToken)
	{
		Invoice invoice = new()
		{
			Number = request.Number,
			StoreId = request.StoreId,
			VendorId = request.VendorId,
			Status = new InvoiceStatus
			{
				CreatedAt = DateTime.Now,
				State = State.Active
			},
			CreatedAt = DateTime.Now
		};

		await _invoiceRepository.CreateAsync(invoice, cancellationToken);

		Transaction transaction = new()
		{
			Name = "НВ" + request.Number,
			Description = "Відкриття накладної №" + request.Number,
			Money = new Money(request.Value),
			CreatedAt = DateTime.Now,
			StoreId = request.StoreId,
			InvoiceId = invoice.Id,
			TransactionTypeId = 1
		};

		await _transactionRepository.CreateAsync(transaction, cancellationToken);

		return invoice;
	}
}