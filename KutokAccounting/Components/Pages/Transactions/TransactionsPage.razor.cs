using KutokAccounting.Components.Pages.Transactions.Models;
using KutokAccounting.Components.Pages.TransactionTypes.Models;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Helpers;
using KutokAccounting.Services.Stores.Models;
using KutokAccounting.Services.Transactions.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Transactions;

public partial class TransactionsPage : ComponentBase
{
	[Parameter]
	public int StoreId { get; set; }
	
	private readonly HashSet<string> _stringFilterOperators = new()
	{
		"equals"
	};

	private readonly HashSet<string> _intFilterOperators = new()
	{
		"=",
		">",
		"<"
	};

	private MudDataGrid<TransactionView> _dataGrid = new();

	private CalculationResultView _calculationResultView;

	private MudDateRangePicker _picker = new();

	private string? _searchString;

	private DateRange? _dateRange;
	
	protected override Task OnInitializedAsync()
	{
		if (_dateRange == null)
		{
			var now = DateTime.Now;
			
			_dateRange = new DateRange(new DateTime(now.Year, now.Month, 1), new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1));
		}
		
		return Task.CompletedTask;
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender && _dateRange is not null)
		{
			await RefreshCalculationResultView();
		}
	}

	private TransactionQueryParameters BuildQuery(GridState<TransactionView> state)
	{
		Filters? filters = new();
		Sorting? sorting = new();

		IFilterDefinition<TransactionView>? filterDefinition = state.FilterDefinitions.FirstOrDefault();

		if (filterDefinition is not null)
		{
			if ($"{filterDefinition.Title} - {filterDefinition.Operator}" ==
				$"{TransactionFiltersConstants.Name} - {TransactionFiltersConstants.StringEqualsOperator}")
			{
				filters.Name = filterDefinition?.Value?.ToString();
			}
			else if ($"{filterDefinition.Title} - {filterDefinition.Operator}" ==
				$"{TransactionFiltersConstants.Description} - {TransactionFiltersConstants.StringEqualsOperator}")
			{
				filters.Description = filterDefinition?.Value?.ToString();
			}
			else if ($"{filterDefinition.Title} - {filterDefinition.Operator}" ==
				$"{TransactionFiltersConstants.Value} - {TransactionFiltersConstants.IntegerEqualsOperator}")
			{
				if (long.TryParse(filterDefinition?.Value?.ToString(), out long value))
				{
					filters.Value = new Money(value);
				}
			}
			else if ($"{filterDefinition.Title} - {filterDefinition.Operator}" ==
				$"{TransactionFiltersConstants.Value} - {TransactionFiltersConstants.LessThanOperator}")
			{
				if (long.TryParse(filterDefinition?.Value?.ToString(), out long lessThan))
				{
					filters.LessThan = new Money(lessThan);
				}
			}
			else if ($"{filterDefinition.Title} - {filterDefinition.Operator}" ==
				$"{TransactionFiltersConstants.Value} - {TransactionFiltersConstants.MoreThanOperator}")
			{
				if (long.TryParse(filterDefinition?.Value?.ToString(), out long moreThan))
				{
					filters.MoreThan = new Money(moreThan);
				}
			}
			else if ($"{filterDefinition.Title} - {filterDefinition.Operator}" ==
				$"{TransactionFiltersConstants.TransactionType} - {TransactionFiltersConstants.StringEqualsOperator}")
			{
				if (int.TryParse(filterDefinition?.Value?.ToString(), out int transactionTypeId))
				{
					filters.TransactionTypeId = transactionTypeId;
				}
			}
		}

		SortDefinition<TransactionView>? sortDefinition = state.SortDefinitions.FirstOrDefault();

		if (sortDefinition is not null)
		{
			sorting.SortBy = sortDefinition.SortBy switch
			{
				nameof(TransactionView.Name) => sortDefinition.SortBy,
				nameof(TransactionView.Description) => sortDefinition.SortBy,
				nameof(Money) + "." + nameof(Money.Value) => sortDefinition.SortBy,
				nameof(TransactionView.CreatedAt) => sortDefinition.SortBy,
				_ => sorting.SortBy
			};

			sorting.Descending = sortDefinition.Descending;
		}

		if (_dateRange?.Start.HasValue is true && _dateRange?.End.HasValue is true)
		{
			if (_dateRange.Start.Value.Equals(_dateRange.End.Value))
			{
				filters.Range = DateTimeHelper.GetDayStartEndRange(_dateRange.Start.Value);
			}
			else
			{
				filters.Range = new DateTimeRange(_dateRange.Start.Value, _dateRange.End.Value);
			}
		}

		return new TransactionQueryParameters
		{
			Filters = filters,
			Pagination = new Pagination
			{
				Page = state.Page + 1,
				PageSize = state.PageSize
			},
			SearchString = _searchString,
			Sorting = sorting
		};
	}

	private async Task<GridData<TransactionView>> GetTransactionsAsync(GridState<TransactionView> state)
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));
		
		try
		{
			return await GetTransactionInternalAsync(state, tokenSource.Token);
		}
		catch (Exception)
		{
			return new GridData<TransactionView>
			{
				Items = new List<TransactionView>(),
				TotalItems = 0
			};
		}
		finally
		{
			StateHasChanged();
		}
	}

	private async ValueTask<GridData<TransactionView>> GetTransactionInternalAsync(
		GridState<TransactionView> state,
		CancellationToken cancellationToken)
	{
		TransactionQueryParameters transactionQueryParameters = BuildQuery(state);

		PagedResult<Transaction> pagedResult =
			await TransactionService.GetAsync(transactionQueryParameters, cancellationToken);

		List<TransactionView> view = pagedResult.Items.Select(t => new TransactionView
		{
			Id = t.Id,
			Name = t.Name,
			Description = t.Description,
			Money = t.Money,
			CreatedAt = t.CreatedAt,
			TransactionType = new TransactionTypeView
			{
				Id = t.TransactionType.Id,
				Name = t.TransactionType.Name,
				IsIncome = t.TransactionType.IsIncome
			}
		}).ToList();

		return new GridData<TransactionView>
		{
			Items = view ?? new List<TransactionView>(),
			TotalItems = pagedResult.Count
		};
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
			await DialogService.ShowAsync<AddTransactionDialog>("Додати нову транзакцію", options);

		DialogResult? result = await dialog.Result;

		if (result?.Canceled is false)
		{
			await RefreshCalculationResultView();
			
			await _dataGrid.ReloadServerData();
		}
	}

	private async Task OnDeleteButtonClick(TransactionView transaction)
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		await TransactionService.DeleteAsync(transaction.Id, tokenSource.Token);

		await RefreshCalculationResultView();
		
		await _dataGrid.ReloadServerData();

		StateHasChanged();
	}

	private async Task OnDateRangeChanged()
	{
		await RefreshCalculationResultView();
		await _dataGrid.ReloadServerData();
	}

	private async ValueTask RefreshCalculationResultView()
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		DateTimeRange rangeToCalculate;

		if (_dateRange?.Start.HasValue is true && _dateRange.End.HasValue is true)
		{
			if (_dateRange.Start.Value == _dateRange.End.Value)
			{
				rangeToCalculate = DateTimeHelper.GetDayStartEndRange(_dateRange.Start.Value);
			}
			else
			{
				rangeToCalculate = new DateTimeRange(_dateRange.Start.Value, _dateRange.End.Value);
			}
		}
		else
		{
			rangeToCalculate = new DateTimeRange(DateTime.MinValue, DateTime.MaxValue);
		}
		
		await _calculationResultView.RecalculateResultAsync(rangeToCalculate, tokenSource.Token);
	}

	private async Task OnEditButtonClick(TransactionView transaction)
	{
		DialogOptions options = new()
		{
			FullWidth = true,
			MaxWidth = MaxWidth.Medium,
			CloseButton = true,
			CloseOnEscapeKey = true
		};

		DialogParameters<EditTransactionDialog> parameters = new()
		{
			{
				d => d.TransactionView, transaction
			}
		};

		IDialogReference dialog =
			await DialogService.ShowAsync<EditTransactionDialog>("Редагувати транзакцію", parameters, options);

		DialogResult? result = await dialog.Result;

		if (result?.Canceled is false)
		{
			await RefreshCalculationResultView();
			
			await _dataGrid.ReloadServerData();
		}
	}
}