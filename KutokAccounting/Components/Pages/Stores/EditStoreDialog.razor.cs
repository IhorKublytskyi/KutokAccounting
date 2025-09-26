using KutokAccounting.Services.Stores.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Stores;

public partial class EditStoreDialog
{
	private bool _isValid = true;

	[CascadingParameter]
	public IMudDialogInstance MudDialog { get; set; }

	[Parameter]
	public StoreDto Store { get; set; } = null!;

	public DateTime? NullableSetUpDate
	{
		get => Store.SetupDate;
		set
		{
			if (value is null)
			{
				DialogService.ShowMessageBox("Обов'язкові данні не обрані!", $"Оберіть дату відкриття магазину");
			}
			else
			{
				Store.SetupDate = value.Value;
			}
		}
	}

	private void Cancel()
	{
		MudDialog.Close();
	}

	private async Task EditAsync()
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		try
		{
			await StoresService.UpdateAsync(Store.Id, Store, tokenSource.Token);
		}
		catch (Exception e)
		{
			await DialogService.ShowMessageBox("Виникла помилка", $"Текст помилки: {e}");
			Logger.LogError($"Exception occured while editing store. Messgae: {e.Message}");
			MudDialog.Close(DialogResult.Cancel());
		}

		MudDialog.Close(DialogResult.Ok(true));
	}
}