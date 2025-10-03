using KutokAccounting.Services.Stores.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Stores;

public partial class SearchStoreDialog
{
	private bool _isValid;

	[Parameter]
	public StoreSearchParameters StoreSearchParameters { get; set; }

	[CascadingParameter]
	public IMudDialogInstance MudDialog { get; set; }

	private void Cancel()
	{
		MudDialog.Cancel();
	}

	private void Search()
	{
		StoreSearchParameters storeSearchParameters = new();

		if (string.IsNullOrEmpty(StoreSearchParameters?.Name) is false)
		{
			storeSearchParameters.Name = StoreSearchParameters.Name;
		}

		if (string.IsNullOrEmpty(StoreSearchParameters?.Address) is false)
		{
			storeSearchParameters.Address = StoreSearchParameters.Address;
		}

		if (StoreSearchParameters?.IsOpened is not null)
		{
			storeSearchParameters.IsOpened = StoreSearchParameters.IsOpened.Value;
		}

		if (StoreSearchParameters?.SetupDate is not null)
		{
			storeSearchParameters.SetupDate = StoreSearchParameters.SetupDate.Value;
		}

		MudDialog.Close(DialogResult.Ok(storeSearchParameters));
	}

	private void OnNameChanged(string name)
	{
		StoreSearchParameters.Name = name;
	}

	private void OnAddressChanged(string address)
	{
		StoreSearchParameters.Address = address;
	}

	private void OnIsOpenedChanged(bool isOpened)
	{
		StoreSearchParameters.IsOpened = isOpened;
	}

	private void OnOpeningDateChanged(DateTime? setupDate)
	{
		StoreSearchParameters.SetupDate = setupDate?.Date;
	}

	private void ResetParametersAndSearch()
	{
		StoreSearchParameters.Name = string.Empty;
		StoreSearchParameters.Address = string.Empty;
		StoreSearchParameters.SetupDate = null;
		StoreSearchParameters.IsOpened = null;

		Search();
	}
}