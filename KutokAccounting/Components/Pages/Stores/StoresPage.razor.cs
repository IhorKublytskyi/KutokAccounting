
using KutokAccounting.Services.Stores.Dtos;
using KutokAccounting.Services.Stores.Models;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Stores;


partial class StoresPage
{
	private MudDataGrid<StoreDto> _dataGrid;
    private string _searchString = String.Empty;
    private readonly DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.Medium,
        CloseButton = true,
        CloseOnEscapeKey = true
    };
    
    public async Task<GridData<StoreDto>> GetStoresAsync(GridState<StoreDto> state)
    {
        var page = new Services.Stores.Models.Page
        {
            PageNumber = state.Page + 1,
            PageSize = state.PageSize
        };
        
        var gridData = new GridData<StoreDto>
        {
            Items = new List<StoreDto>(),
            TotalItems = 0
        };

        var searchParameters = new SearchParameters
        {
            Address = _searchString,
            Name = _searchString,
        };
        try
        {
            var storesPage = await StoresService.GetStoresPageAsync(page, Cts.Token, searchParameters);
            
            gridData.Items = storesPage.Items;
            gridData.TotalItems = storesPage.Count;
        }
        catch (Exception e)
        {
            await DialogService.ShowMessageBox("Виникла помилка", $"Текст помилки: {e}");
            Logger.LogError($"Exception occured while retreiving stores from db. Exception message: {e.Message}");
        }
        finally
        {
            StateHasChanged();
        }

        return gridData;
    }
    public async Task OnAddShopButtonClick()
    {
        var dialog = await DialogService.ShowAsync<AddStoreDialog>("Додати магазин", _dialogOptions);
        var dialogResult = await dialog.Result;
        
        
        if (_dataGrid != null && !dialogResult.Canceled)
            await _dataGrid.ReloadServerData();
    }

    public async Task OnEditButtonClick(StoreDto? storeDto)
    {
        var parameters = new DialogParameters<EditStoreDialog> { { d => d.Store, storeDto } };
        
        var dialog = await DialogService.ShowAsync<EditStoreDialog>("Редагувати данні магазину", parameters, _dialogOptions);
        var dialogResult = await dialog.Result;
        
        if (_dataGrid != null && !dialogResult.Canceled)
            await _dataGrid.ReloadServerData();
    }

    private async Task OnDeleteButtonClick(StoreDto? storeDto)
    { 
        var shouldBeDeleted = await DialogService.ShowMessageBox("Увага!", $"Ви точно хочете видалити магазин {storeDto.Name}?", yesText:"Так", noText:"Ні");
        
        if (shouldBeDeleted.GetValueOrDefault())
        {
            await TryDeleteStoreAsync(storeDto);
        }
    }

    private async Task TryDeleteStoreAsync(StoreDto storeDto)
    {
        try
        {
            await StoresService.DeleteStoreAsync(storeDto.Id, Cts.Token);
            
            if (_dataGrid != null)
                await _dataGrid.ReloadServerData();
        }
        catch (Exception e)
        {
            Logger.LogError($"Exception occured while deleting store. Messgae: {e.Message}");
            await DialogService.ShowMessageBox("Виникла помилка", $"Текст помилки: {e}");
        }
    }
    private async Task OnMoreButtonClick(StoreDto storeDto)
    {
        var parameters = new DialogParameters<EditStoreDialog> { { d => d.Store, storeDto } };
        await DialogService.ShowAsync<MoreStoreInfoDialog>("Більше інформації про магазин", parameters);
    }
}