using KutokAccounting.Services.Vendors.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Vendors;

public partial class AddVendorDialog
{
	private string? _name;
	private string? _description;

	private bool _isSuccess;

	[CascadingParameter]
	public IMudDialogInstance MudDialog { get; set; }

	private async Task AddAsync()
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		VendorDto vendor = new()
		{
			Name = _name,
			Description = _description
		};

		await VendorService.CreateAsync(vendor, tokenSource.Token);

		MudDialog.Close(DialogResult.Ok(true));
	}

	private void Cancel()
	{
		MudDialog.Close();
	}
}