
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Dtos;
using KutokAccounting.Services.Stores.Models;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Stores;


partial class StoresPage
{
	private MudDataGrid<StoreDto> _dataGrid;
    private StoreSearchParameters? _searchStoreParameters = new();
    private readonly DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.Medium,
        CloseButton = true,
        CloseOnEscapeKey = true
    };
    
    public async Task<GridData<StoreDto>> GetStoresAsync(GridState<StoreDto> state)
    {
        var gridData = new GridData<StoreDto>
        {
            Items = new List<StoreDto>(),
            TotalItems = 0
        };
        try
        {
            using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

            var pagination = new Pagination
            {
                Page = state.Page + 1,
                PageSize = state.PageSize
            };

            
            var storesPage = await StoresService.GetPageAsync(pagination, _searchStoreParameters, tokenSource.Token);

            gridData.Items = storesPage.Items;
            gridData.TotalItems = storesPage.Count;
        }
        catch (Exception e)
        {
            await DialogService.ShowMessageBox("Виникла помилка", $"Текст помилки: {e}");
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
        try
        {
            using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

            var parameters = new DialogParameters<EditStoreDialog>
            {
                { d => d.Store, storeDto }
            };
        
            var dialog = await DialogService.ShowAsync<EditStoreDialog>("Редагувати данні магазину", parameters, _dialogOptions);
            var dialogResult = await dialog.Result;
        
            if (_dataGrid != null && !dialogResult.Canceled)
                await _dataGrid.ReloadServerData();
        }
        catch (Exception e)
        {
            await DialogService.ShowMessageBox("Exception", $"{e.Message}");
        }
        
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
        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

        try
        {
            await StoresService.DeleteAsync(storeDto.Id, tokenSource.Token);
            
            if (_dataGrid != null)
                await _dataGrid.ReloadServerData();
        }
        catch (Exception e)
        {
            await DialogService.ShowMessageBox("Виникла помилка", $"Текст помилки: {e}");
        }
    }
    private async Task OnMoreButtonClick(StoreDto storeDto)
    {
        var parameters = new DialogParameters<EditStoreDialog> { { d => d.Store, storeDto } };
        await DialogService.ShowAsync<MoreStoreInfoDialog>("Більше інформації про магазин", parameters, _dialogOptions);
    }

    private async Task OnSearchButtonClick()
    {
        var dialogParameters = new DialogParameters<SearchStoreDialog> { {d => d.StoreSearchParameters, _searchStoreParameters} };
        
        var dialog = await DialogService.ShowAsync<SearchStoreDialog>("Пошук", dialogParameters, _dialogOptions);
        var dialogResult = await dialog.Result;
        
        if (dialogResult!.Data != null && dialogResult.Data is StoreSearchParameters storeProperties)
        {
            await DialogService.ShowMessageBox("reloaded", $"{storeProperties.Name}, {storeProperties.Address}, {storeProperties.SetupDate}, {storeProperties.IsOpened}");
            _searchStoreParameters = storeProperties;
        }
        
        if (_dataGrid != null && !dialogResult.Canceled)
            await _dataGrid.ReloadServerData();
    }
}