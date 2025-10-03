using KutokAccounting.Services.Stores.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Stores;

public partial class SearchStoreDialog
{
	private bool _isValid;

	[Parameter]
	public StoreQueryParameters StoreQueryParameters { get; set; }

	[CascadingParameter]
	public IMudDialogInstance MudDialog { get; set; }

	private void Cancel()
	{
		MudDialog.Cancel();
	}

	private void Search()
	{
		StoreQueryParameters storeQueryParameters = new();

		if (string.IsNullOrEmpty(StoreQueryParameters?.Name) is false)
		{
			storeQueryParameters.Name = StoreQueryParameters.Name;
		}

		if (string.IsNullOrEmpty(StoreQueryParameters?.Address) is false)
		{
			storeQueryParameters.Address = StoreQueryParameters.Address;
		}

		if (StoreQueryParameters?.IsOpened is not null)
		{
			storeQueryParameters.IsOpened = StoreQueryParameters.IsOpened.Value;
		}

		if (StoreQueryParameters?.SetupDate is not null)
		{
			storeQueryParameters.SetupDate = StoreQueryParameters.SetupDate.Value;
		}

		MudDialog.Close(DialogResult.Ok(storeQueryParameters));
	}

	private void OnNameChanged(string name)
	{
		StoreQueryParameters.Name = name;
	}

	private void OnAddressChanged(string address)
	{
		StoreQueryParameters.Address = address;
	}

	private void OnIsOpenedChanged(bool isOpened)
	{
		StoreQueryParameters.IsOpened = isOpened;
	}

	private void OnOpeningDateChanged(DateTime? setupDate)
	{
		StoreQueryParameters.SetupDate = setupDate?.Date;
	}

	private void ResetParametersAndSearch()
	{
		StoreQueryParameters.Name = string.Empty;
		StoreQueryParameters.Address = string.Empty;
		StoreQueryParameters.SetupDate = null;
		StoreQueryParameters.IsOpened = null;

		Search();
	}
}