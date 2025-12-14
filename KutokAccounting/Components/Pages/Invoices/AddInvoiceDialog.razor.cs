using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Invoices.Interfaces;
using KutokAccounting.Services.Invoices.Models;
using KutokAccounting.Services.Vendors;
using KutokAccounting.Services.Vendors.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Invoices;

public partial class AddInvoiceDialog : ComponentBase
{
	private string[] _errors;

	private string? _number;
	private string _value = string.Empty;
	private Vendor _vendor;

	private bool _isSuccess;

	[Inject]
	public IInvoiceService InvoiceService { get; set; }

	[Inject]
	public IVendorService VendorService { get; set; }

	[CascadingParameter]
	public IMudDialogInstance MudDialog { get; set; }

	[Parameter]
	public int StoreId { get; set; }

	private async Task AddAsync()
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		InvoiceDto invoice = new()
		{
			Number = _number,
			StoreId = StoreId,
			VendorId = _vendor.Id,
			Value = Money.Parse(_value).Value
		};

		await InvoiceService.CreateAsync(invoice, tokenSource.Token);

		MudDialog.Close(DialogResult.Ok(true));
	}

	private async Task<IEnumerable<Vendor>> Search(string value, CancellationToken cancellationToken)
	{
		VendorQueryParameters parameters = new(null, Pagination: new Pagination
		{
			Page = 1,
			PageSize = 10
		}, SearchString: value);

		PagedResult<Vendor> pagedResult = await VendorService.GetAsync(parameters, cancellationToken);

		return pagedResult.Items;
	}

	private string ValidateValue(string value)
	{
		return MoneyFormatRegex.MoneyValueRegex().IsMatch(value)
			? string.Empty
			: "Значення повинно бути додатним числом з двома знаками після точки (наприклад, 123.45 або 123,45).";
	}

	private void Cancel()
	{
		MudDialog.Close();
	}
}