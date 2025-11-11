using KutokAccounting.Components.Pages.TransactionTypes.Models;
using KutokAccounting.Services.TransactionTypes.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KutokAccounting.Components.Pages.TransactionTypes;

public partial class EditTransactionTypeDialog
{
	private bool _isSuccess;

	[CascadingParameter]
	public IMudDialogInstance MudDialog { get; set; }

	[Parameter]
	public TransactionTypeView? TransactionTypeView { get; set; }

	private async Task UpdateAsync()
	{
		using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(30));

		TransactionTypeDto transactionType = new()
		{
			Id = TransactionTypeView.Id,
			Name = TransactionTypeView.Name,
			IsIncome = TransactionTypeView.IsIncome
		};

		await TransactionTypeService.UpdateAsync(transactionType, tokenSource.Token);

		MudDialog.Close(DialogResult.Ok(true));
	}

	private void Cancel()
	{
		MudDialog.Close();
	}
}