using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Vendors.Models;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Vendors;

public partial class VendorsPage
{
    private MudDataGrid<Vendor> _dataGrid = new();
    private readonly HashSet<string> _filterOperators = new()
    {
        "equals"
    };

    private int _page = 1;
    private int _pageSize = 10;

    private string? _searchString;

    private async Task<GridData<Vendor>> GetVendorsAsync(GridState<Vendor> state)
    {
        using CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        var filter = state.FilterDefinitions.FirstOrDefault()?.Value?.ToString();

        _page += state.Page;
        _pageSize = state.PageSize;

        var queryParameters = new VendorQueryParameters(filter, _searchString, new Pagination() { Page = _page, PageSize = _pageSize });

        try
        {
            PagedResult<Vendor> pagedResult = await VendorService.GetAsync(queryParameters, tokenSource.Token);

            return new GridData<Vendor>
            {
                Items = pagedResult.Items ?? new List<Vendor>(),
                TotalItems = pagedResult.Count
            };
        }
        catch (Exception)
        {
            return new GridData<Vendor>
            {
                Items = new List<Vendor>(),
                TotalItems = 0
            };
        }
        finally
        {
            StateHasChanged();
        }
    }

    private async Task OnDeleteButtonClick(Vendor vendor)
    {
        using CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        await VendorService.DeleteAsync(vendor.Id, tokenSource.Token);

        if (_dataGrid != null)
            await _dataGrid.ReloadServerData();
    }

    private async Task OnEditButtonClick(Vendor vendor)
    {
        DialogOptions options = new DialogOptions
        {
            FullWidth = true,
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        DialogParameters<EditVendorDialog> parameters = new DialogParameters<EditVendorDialog> { { d => d.Vendor, vendor } };

        IDialogReference dialog = await DialogService.ShowAsync<EditVendorDialog>("Редагувати постачальника", parameters, options);

        DialogResult? result = await dialog.Result;

        if (_dataGrid != null && !result.Canceled)
            await _dataGrid.ReloadServerData();
    }

    private async Task OnAddButtonClick()
    {
        DialogOptions options = new DialogOptions
        {
            FullWidth = true,
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        IDialogReference dialog = await DialogService.ShowAsync<AddVendorDialog>("Додати нового постачальника", options);

        DialogResult? result = await dialog.Result;

        if (_dataGrid != null && !result.Canceled)
            await _dataGrid.ReloadServerData();
    }
}
