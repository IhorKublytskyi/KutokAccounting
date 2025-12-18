using KutokAccounting.Components.Pages.Invoices.Models;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Invoices.Interfaces;
using KutokAccounting.Services.Invoices.Models;
using KutokAccounting.Services.Vendors;
using KutokAccounting.Services.Vendors.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Invoices;

public partial class EditInvoiceDialog : ComponentBase
{
	public string _value = string.Empty;
	public bool _isSuccess;

	[Parameter]
	public int StoreId { get; set; }

	[Parameter]
	public InvoiceView Invoice { get; set; }

	[Inject]
	public required ISnackbar Snackbar { get; set; }
	
	[Inject]
	public required IVendorService VendorService { get; set; }

	[Inject]
	public required IInvoiceService InvoiceService { get; set; }

	[CascadingParameter]
	public required IMudDialogInstance MudDialog { get; set; }

	public async Task EditAsync()
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));
		
		InvoiceDto invoice = new()
		{
			Id = Invoice.Id,
			Number = Invoice.Number,
			StoreId = StoreId,
			VendorId = Invoice.Vendor.Id,
			Money = Money.Parse(_value)
		};

		await InvoiceService.UpdateAsync(invoice, tokenSource.Token);

		MudDialog.Close(DialogResult.Ok(true));
	}

	public void Cancel()
	{
		MudDialog.Close();
	}

	private string ValidateValue(string value)
	{
		return MoneyFormatRegex.MoneyValueRegex().IsMatch(value)
			? string.Empty
			: "Значення повинно бути додатним числом з двома знаками після точки (наприклад, 123.45 або 123,45).";
	}
	
	private async Task<IEnumerable<Vendor>> SearchAsync(string value, CancellationToken cancellationToken)
	{
		VendorQueryParameters parameters = new(null, Pagination: new Pagination
		{
			Page = 1,
			PageSize = 10
		}, SearchString: value);

		PagedResult<Vendor> pagedResult = await VendorService.GetAsync(parameters, cancellationToken);

		return pagedResult.Items;
	}
}