using System.Text.RegularExpressions;
using KutokAccounting.Components.Pages.TransactionTypes.Models;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Transactions.Models;
using KutokAccounting.Services.TransactionTypes.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Transactions;

public partial class AddTransactionDialog : ComponentBase
{
	private string[] _errors;

	private string? _name;

	private string? _description;

	private string _value = string.Empty;

	private TransactionTypeView _transactionType;

	private bool _isSuccess;

	[Parameter]
	public int StoreId { get; set; }
	
	[CascadingParameter]
	public IMudDialogInstance MudDialog { get; set; }

	private async Task AddAsync()
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));
		
		TransactionDto transaction = new()
		{
			Name = _name,
			Description = _description,
			Value = Money.Parse(_value).Value,
			TransactionTypeId = _transactionType.Id,
			StoreId = StoreId,
			InvoiceId = 1 // Заглушка
		};

		await TransactionService.CreateAsync(transaction, tokenSource.Token);

		MudDialog.Close(DialogResult.Ok(true));
	}

	private async Task<IEnumerable<TransactionTypeView>> Search(string value, CancellationToken cancellationToken)
	{
		TransactionTypeQueryParameters parameters = new(new Services.TransactionTypes.Models.Filters(), value,
			new Pagination
			{
				Page = 1,
				PageSize = 10
			});

		PagedResult<TransactionType> pagedResult =
			await TransactionTypeService.GetAsync(parameters, cancellationToken);

		List<TransactionTypeView> view = pagedResult.Items
			.Select(tp => new TransactionTypeView
			{
				Id = tp.Id,
				Name = tp.Name,
				IsIncome = tp.IsIncome
			}).ToList();

		return view;
	}

	private string ValidateValue(string value)
	{
		Regex regex = new(@"^(0|[1-9]\d*)(\.|,)([1-9]|\d\d)$");

		return regex.Match(value) is not {Success: true}
			? "Значення повинно бути додатним числом з двома знаками після точки (наприклад, 123.45 або 123,45)."
			: string.Empty;
	}

	private void Cancel()
	{
		MudDialog.Close();
	}
}