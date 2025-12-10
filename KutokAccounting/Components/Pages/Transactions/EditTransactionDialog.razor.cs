using System.Text.RegularExpressions;
using KutokAccounting.Components.Pages.Transactions.Models;
using KutokAccounting.Components.Pages.TransactionTypes.Models;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Transactions.Models;
using KutokAccounting.Services.TransactionTypes.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Transactions;

public partial class EditTransactionDialog : ComponentBase
{
	private bool _isSuccess;

	private TransactionTypeView _transactionType;

	[CascadingParameter]
	public IMudDialogInstance MudDialog { get; set; }

	[Parameter]
	public TransactionView? TransactionView { get; set; }

	private async Task UpdateAsync()
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		TransactionDto transaction = new()
		{
			Id = TransactionView.Id,
			Name = TransactionView.Name,
			Description = TransactionView.Description,
			Value = TransactionView.Money.Value,
			StoreId = TransactionView.StoreId,
			TransactionTypeId = TransactionView.TransactionType.Id
		};

		await TransactionService.UpdateAsync(transaction, tokenSource.Token);

		MudDialog.Close(DialogResult.Ok(true));
	}

	private async Task<IEnumerable<TransactionTypeView>> SearchAsync(string value, CancellationToken cancellationToken)
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		TransactionTypeQueryParameters parameters = new(
			new Services.TransactionTypes.Models.Filters(), value, new Pagination
			{
				Page = 1,
				PageSize = 10
			});

		PagedResult<TransactionType> pagedResult =
			await TransactionTypeService.GetAsync(parameters, tokenSource.Token);

		List<TransactionTypeView> view = pagedResult.Items
			.Select(tp => new TransactionTypeView
			{
				Id = tp.Id,
				Name = tp.Name,
				IsIncome = tp.IsIncome
			}).ToList();

		return view;
	}

	private void OnMoneyChanged(string value)
	{
		try
		{
			TransactionView.Money = Money.Parse(value);
		}
		catch (Exception e)
		{
			TransactionView.Money = default;
		}
	}

	private string ValidateValue(string value)
	{
		return MoneyFormatRegex.MoneyValueRegex().IsMatch(value)
			? "Значення повинно бути додатним числом з двома знаками після точки (наприклад, 123.45 або 123,45)."
			: string.Empty;
	}

	private void Cancel()
	{
		MudDialog.Close();
	}
}