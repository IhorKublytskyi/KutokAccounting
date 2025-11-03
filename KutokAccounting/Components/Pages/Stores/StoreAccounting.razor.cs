using KutokAccounting.Services.Stores.Dtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Stores;

public partial class StoreAccounting : ComponentBase
{
	private int _storeId;

	[Parameter]
	public int StoreId
	{
		get => _storeId;
		set
		{
			_storeId = value;
			_ = GetStoreWithIdAsync();
		}
	}

	private StoreDto Store { get; set; }

	private async ValueTask GetStoreWithIdAsync()
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		try
		{
			Store = await StoreService.GetByIdAsync(StoreId, tokenSource.Token);
		}
		catch (ArgumentException)
		{
			MudDialog dialog = new();
			await dialog.ShowAsync($"Store with id {StoreId} does not exist");
		}
		catch (Exception ex)
		{
			MudDialog dialog = new();
			await dialog.ShowAsync($"Exception occurred: {ex.Message}");
		}
	}
}