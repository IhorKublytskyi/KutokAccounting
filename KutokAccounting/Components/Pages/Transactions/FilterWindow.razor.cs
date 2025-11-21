using System.Text.RegularExpressions;
using KutokAccounting.Components.Pages.Transactions.Models;
using KutokAccounting.DataProvider.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Transactions;

public partial class FilterWindow
{
	private string _icon = Icons.Material.Outlined.FilterAlt;
	private bool _filterOpen;
	private string _operator = "=";
	private string _rawValue = string.Empty;
	private FilterDefinition<TransactionView>? _currentMoneyFilter;

	[Parameter]
	public required HashSet<string> Operators { get; set; }

	[Parameter]
	public required FilterContext<TransactionView> Context { get; set; }

	protected override void OnInitialized()
	{
		_operator = Operators.FirstOrDefault() ?? "=";
	}

	private void OpenFilter()
	{
		IFilterDefinition<TransactionView>? existingFilter =
			Context.FilterDefinitions?.FirstOrDefault(f => f.Title == TransactionFiltersConstants.Value);

		if (existingFilter != null)
		{
			if (long.TryParse(existingFilter.Value.ToString(), out long result))
			{
				_operator = existingFilter.Operator ?? "=";
				_rawValue = new Money(result).ToString("0.00");
			};
		}

		_filterOpen = true;
	}

	private async Task ClearFilterAsync()
	{
		if (_currentMoneyFilter != null)
		{
			await Context.Actions.ClearFilterAsync(_currentMoneyFilter);
			_currentMoneyFilter = null;
		}

		_operator = "=";
		_rawValue = string.Empty;
		_icon = Icons.Material.Outlined.FilterAlt;
		_filterOpen = false;

		StateHasChanged();
	}

	private async Task ApplyFilterAsync()
	{
		Money money = Money.ConvertFromStringToMoney(_rawValue);

		if (_currentMoneyFilter != null)
		{
			await Context.Actions.ClearFilterAsync(_currentMoneyFilter);
		}

		_currentMoneyFilter = new FilterDefinition<TransactionView>
		{
			Title = TransactionFiltersConstants.Value,
			Operator = _operator,
			Value = money.Value
		};

		await Context.Actions.ApplyFilterAsync(_currentMoneyFilter);

		_filterOpen = false;
		_icon = Icons.Material.Filled.FilterAlt;

		StateHasChanged();
	}

	private string ValidateValue(string value)
	{
		Regex regex = new(@"^(0|[1-9]\d*)(\.|,)([1-9]|\d\d)$");

		return regex.IsMatch(value)
			? string.Empty
			: "Число має бути у форматі 123.45 або 123,45";
	}
}