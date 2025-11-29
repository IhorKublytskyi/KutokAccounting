using KutokAccounting.Components.Pages.Transactions.Helpers;
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
	private readonly HashSet<string> _stringFilterOperators = ["equals"];

	private readonly HashSet<string> _intFilterOperators =
	[
		"=",
		">",
		"<"
	];

	private MudDataGrid<TransactionView> _dataGrid = new();

	private MudDateRangePicker _picker = new();

	private string? _searchString;

	private DateRange? _dateRange;

	private DateTimeRange _rangeToCalculate;

	[Parameter]
	public int StoreId { get; set; }

	[Inject]
	public required TransactionsStateNotifier TransactionsStateNotifier { get; set; }

	protected override void OnInitialized()
	{
		if (_dateRange is not null)
		{
			return;
		}

		DateTime now = DateTime.Now;

		_dateRange = new DateRange(new DateTime(now.Year, now.Month, 1),
			new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1));

		_rangeToCalculate = NormalizeDateRange(_dateRange);
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await TransactionsStateNotifier.TransactionsChangedAsync();
	}

	protected override async Task OnParametersSetAsync()
	{
		await _dataGrid.ReloadServerData();
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
	}

	private async ValueTask<GridData<TransactionView>> GetTransactionInternalAsync(
		GridState<TransactionView> state,
		CancellationToken cancellationToken)
	{
		QueryMapper mapper = new QueryMapper(StoreId, _searchString, _dateRange);
		
		TransactionQueryParameters transactionQueryParameters = mapper.MapToQueryParameters(state);

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

	private async Task OnDateRangeChanged()
	{
		_rangeToCalculate = NormalizeDateRange(_dateRange);

		await _dataGrid.ReloadServerData();
	}

	private DateTimeRange NormalizeDateRange(DateRange? dateRange)
	{
		if (dateRange is not {Start: not null, End: not null})
		{
			return new DateTimeRange(DateTime.MinValue, DateTime.MaxValue);
		}

		return dateRange.Start.Value == dateRange.End.Value
			? DateTimeHelper.GetDayStartEndRange(dateRange.Start.Value)
			: new DateTimeRange(dateRange.Start.Value, dateRange.End.Value);
	}

	private async Task OnAddButtonClick()
	{
		DialogParameters<AddTransactionDialog> parameters = new()
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
			await DialogService.ShowAsync<AddTransactionDialog>("Додати нову транзакцію", parameters, options);

		DialogResult? result = await dialog.Result;

		if (result?.Canceled is false)
		{
			await _dataGrid.ReloadServerData();
		}
	}

	private async Task OnDeleteButtonClick(TransactionView transaction)
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		await TransactionService.DeleteAsync(transaction.Id, tokenSource.Token);

		await _dataGrid.ReloadServerData();
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
			await _dataGrid.ReloadServerData();
		}
	}
}