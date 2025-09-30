using KutokAccounting.Components.Pages.TransactionTypes.Models;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.TransactionTypes.Models;
using MudBlazor;

namespace KutokAccounting.Components.Pages.TransactionTypes;

public partial class TransactionTypesPage
{
	private readonly HashSet<string> _filterOperators = new()
	{
		"equals"
	};

	private MudDataGrid<TransactionTypeView> _dataGrid = new();

	private int _page = 1;
	private int _pageSize = 10;

	private string? _searchString;

	private TransactionTypeQueryParameters BuildQuery(GridState<TransactionTypeView> state)
	{
		Filters? filters = new();

		ICollection<IFilterDefinition<TransactionTypeView>> filterDefinitions = state.FilterDefinitions;

		if (filterDefinitions is not null)
		{
			foreach (var filter in filterDefinitions)
			{
				if (filter.Title == "Назва типу транзакції")
				{
					filters.Name = filter?.Value?.ToString();
				}
				else if (filter.Title == TransactionTypeFiltersConstants.Name)
				{
					filters.IsIncome = filter.Value is TransactionTypeFiltersConstants.Expense ? false : true;
				}
			}
		}

		return new TransactionTypeQueryParameters(filters, _searchString, new Pagination
		{
			Page = _page,
			PageSize = _pageSize
		});
	}

	private async Task<GridData<TransactionTypeView>> GetTransactionTypesAsync(GridState<TransactionTypeView> state)
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		try
		{
			return await GetTransactionTypeInternalAsync(state, tokenSource.Token);
		}
		catch (Exception)
		{
			return new GridData<TransactionTypeView>
			{
				Items = new List<TransactionTypeView>(),
				TotalItems = 0
			};
		}
		finally
		{
			StateHasChanged();
		}
	}

	private async ValueTask<GridData<TransactionTypeView>> GetTransactionTypeInternalAsync(
		GridState<TransactionTypeView> state,
		CancellationToken cancellationToken)
	{
		string?[] filter = state.FilterDefinitions.Select(f => f?.Value?.ToString()).ToArray();

		_page += state.Page;
		_pageSize = state.PageSize;

		TransactionTypeQueryParameters transactionTypeQueryParameters = BuildQuery(state);

		PagedResult<TransactionType> pagedResult =
			await TransactionTypeService.GetAsync(transactionTypeQueryParameters, cancellationToken);

		List<TransactionTypeView> view = pagedResult.Items.Select(tp => new TransactionTypeView
		{
			Id = tp.Id,
			Name = tp.Name,
			IsIncome = tp.IsIncome
		}).ToList();

		return new GridData<TransactionTypeView>
		{
			Items = view ?? new List<TransactionTypeView>(),
			TotalItems = view.Count
		};
	}

	private async Task OnDeleteButtonClick(TransactionTypeView transactionType)
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		await TransactionTypeService.DeleteAsync(transactionType.Id, tokenSource.Token);

		if (_dataGrid is not null)
		{
			await _dataGrid.ReloadServerData();
		}
	}

	private async Task OnEditButtonClick(TransactionTypeView transactionType)
	{
		DialogOptions options = new()
		{
			FullWidth = true,
			MaxWidth = MaxWidth.Medium,
			CloseButton = true,
			CloseOnEscapeKey = true
		};

		DialogParameters<EditTransactionTypeDialog> parameters = new()
		{
			{
				d => d.TransactionTypeView, transactionType
			}
		};

		IDialogReference dialog =
			await DialogService.ShowAsync<EditTransactionTypeDialog>("Редагувати тип транзакції", parameters, options);

		DialogResult? result = await dialog.Result;

		if (_dataGrid is not null && result.Canceled is false)
		{
			await _dataGrid.ReloadServerData();
		}
	}

	private async Task OnAddButtonClick()
	{
		DialogOptions options = new()
		{
			FullWidth = true,
			MaxWidth = MaxWidth.Medium,
			CloseButton = true,
			CloseOnEscapeKey = true
		};

		IDialogReference dialog =
			await DialogService.ShowAsync<AddTransactionTypeDialog>("Додати новий тип транзакції", options);

		DialogResult? result = await dialog.Result;

		if (_dataGrid is not null && result.Canceled is false)
		{
			await _dataGrid.ReloadServerData();
		}
	}
}