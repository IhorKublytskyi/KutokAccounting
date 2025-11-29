using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Stores.Models;
using KutokAccounting.Services.Transactions.Interfaces;
using Microsoft.AspNetCore.Components;

namespace KutokAccounting.Components.Pages.Transactions;

public partial class CalculationResultView : ComponentBase
{
	private CalculationResult? _result = new();

	[Parameter]
	public DateTimeRange Range { get; set; }

	[Inject]
	public required ITransactionService TransactionService { get; set; }

	public async Task RecalculateResultAsync(DateTimeRange range, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		_result = new CalculationResult();

		await TransactionService.CalculateAsync(_result, range, cancellationToken);

		await InvokeAsync(StateHasChanged);
	}
}