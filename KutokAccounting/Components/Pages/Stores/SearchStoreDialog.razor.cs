using KutokAccounting.Services.Stores.Dtos;
using KutokAccounting.Services.Stores.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Stores;

public partial class SearchStoreDialog
{
	private string? _storeNameString;
	private string? _storeAddressString;
	private bool? _isOpened;
	private DateTime? _openingDate;
	private bool _isValid;
	
	private bool _isNameChanged = false;
	private bool _isAddressChanged = false;
	private bool _isOpenedCheckboxChanged = false;
	private bool _isSetupDateChanged = false;

	
	[CascadingParameter]
	public IMudDialogInstance MudDialog { get; set; }
	
	private void Cancel()
	{
		MudDialog.Cancel();
	}

	private void Search()
	{
		StoreSearchParameters storeToSearch = new();

		if (string.IsNullOrEmpty(_storeNameString) is false && _isNameChanged)
		{
			storeToSearch.Name = _storeNameString;
		}

		if (string.IsNullOrEmpty(_storeAddressString) is false && _isAddressChanged)
		{
			storeToSearch.Address = _storeAddressString;
		}

		if (_isOpened is not null && _isOpenedCheckboxChanged)
		{
			storeToSearch.IsOpened = _isOpened.Value;
		}

		if (_openingDate is not null && _isSetupDateChanged)
		{
			storeToSearch.SetupDate = _openingDate.Value;
		}
		
		MudDialog.Close(DialogResult.Ok(storeToSearch));
	}

	private void OnNameChanged(string name)
	{
		_storeNameString = name;
		_isNameChanged = true;
	}

	private void OnAddressChanged(string address)
	{
		_storeAddressString = address;
		_isAddressChanged = true;
	}

	private void OnIsOpenedChanged(bool isOpened)
	{
		_isOpened = isOpened;
		_isOpenedCheckboxChanged = true;
	}

	private void OnOpeningDateChanged(DateTime? setupDate)
	{
		_openingDate = setupDate;
		_isSetupDateChanged = true;
	}
}