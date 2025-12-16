using KutokAccounting.Components.Pages.Invoices.Models;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Invoices.Interfaces;
using KutokAccounting.Services.Invoices.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Invoices;

public partial class InvoicesPage : ComponentBase
{
    private readonly HashSet<string> _stringFilterOperators = ["equals"];
    private MudDataGrid<InvoiceView> _dataGrid = new();
    private string? _searchString;
    private MudDateRangePicker _picker = new();
    private DateRange? _dateRange;
    
    [Inject]
    public required IDialogService DialogService { get; set; }
    
    [Inject]
    public required IInvoiceService InvoiceService { get; set; }
    
    [Parameter]
    public int StoreId { get; set; }

    public async Task<GridData<InvoiceView>> GetInvoicesAsync(GridState<InvoiceView> state)
    {
        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));
        
        try
        {
            return await GetInvoicesInternalAsync(state, tokenSource.Token);
        }
        catch (Exception)
        {
            return new GridData<InvoiceView>
            {
                Items = new List<InvoiceView>(),
                TotalItems = 0
            };
        }
    }
    
    private async ValueTask<GridData<InvoiceView>> GetInvoicesInternalAsync(
        GridState<InvoiceView> state,
        CancellationToken cancellationToken)
    {
        try
        {
            var invoices = await InvoiceService.GetAsync(new InvoiceQueryParameters()
            {
                Pagination = new Pagination()
                {
                    Page = state.Page + 1,
                    PageSize = state.PageSize
                }
            }, cancellationToken);

            var result = invoices.Items.Select(i => new InvoiceView()
            {
                Id = i.Id,
                Number = i.Number,
                Status = i.Status,
                VendorName = i.Vendor?.Name ?? "Unknown"
            }).ToList();

            return new GridData<InvoiceView>()
            {
                Items = result,
                TotalItems = result.Count
            };
        }
        catch (Exception e)
        {
            return new GridData<InvoiceView>()
            {
                Items = new List<InvoiceView>(),
                TotalItems = 0
            };
        }
    } 
    
    private async Task OnAddButtonClick()
    {
        DialogParameters<AddInvoiceDialog> parameters = new()
        {
            {
                d => d.StoreId, StoreId
            }
        };

        DialogOptions options = new()
        {
            FullWidth = true,
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        IDialogReference dialog =
            await DialogService.ShowAsync<AddInvoiceDialog>("Додати нову накладну", parameters, options);

        DialogResult? result = await dialog.Result;

        if (result?.Canceled is false)
        {
            await _dataGrid.ReloadServerData();
        }
    }
    private async Task OnEditButtonClick(InvoiceView invoice)
    {
        
    }
    private async Task OnDeleteButtonClick(InvoiceView invoice)
    {
        
    }
    private async Task OnCloseButtonClick(InvoiceView invoice)
    {
        
    }
    private async Task OnDateRangeChanged()
    {
        
    }
}  
