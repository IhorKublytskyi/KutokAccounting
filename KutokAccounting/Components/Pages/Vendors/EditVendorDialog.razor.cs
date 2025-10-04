using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Vendors.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Vendors;

public partial class EditVendorDialog
{
	private bool _isSuccess;

	[CascadingParameter]
	public IMudDialogInstance MudDialog { get; set; }

	[Parameter]
	public Vendor? Vendor { get; set; }

	private async Task Update()
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		VendorDto vendor = new()
		{
			Id = Vendor.Id,
			Name = Vendor.Name,
			Description = Vendor.Description
		};

		await VendorService.UpdateAsync(vendor, tokenSource.Token);

		MudDialog.Close(DialogResult.Ok(true));
	}

	private void Cancel()
	{
		MudDialog.Close();
	}
}