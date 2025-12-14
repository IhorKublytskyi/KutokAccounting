using KutokAccounting.Components.Pages.Invoices.Models;
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
        return new GridData<InvoiceView>
        {
            Items =  new List<InvoiceView>(),
            TotalItems = 0
        };
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
    private async Task OnDateRangeChanged()
    {
        
    }
}  
