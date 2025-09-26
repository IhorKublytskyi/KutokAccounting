using KutokAccounting.Services.Stores.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Stores;

public partial class AddStoreDialog
{
	[CascadingParameter]
	public IMudDialogInstance MudDialog { get; set; }

	private bool _isValid = true;
	private string _storeName = String.Empty;
	private DateTime? _openingDate = DateTime.Now;
	private bool _isOpened = true;
	private string? _storeAddress;

	private void Cancel()
	{
		MudDialog.Close();
	}

	private async Task AddStoreAsync()
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		StoreDto store = new()
		{
			Name = _storeName,
			IsOpened = _isOpened,
			SetupDate = _openingDate.GetValueOrDefault(),
			Address = _storeAddress
		};

		try
		{
			await StoresService.CreateAsync(store, tokenSource.Token);
			MudDialog.Close(DialogResult.Ok(true));
		}
		catch (Exception e)
		{
			await DialogService.ShowMessageBox("Виникла помилка", $"Текст помилки: {e}");
			Logger.LogError($"Exception occured while adding store. Messgae: {e.Message}");
		}
	}
}