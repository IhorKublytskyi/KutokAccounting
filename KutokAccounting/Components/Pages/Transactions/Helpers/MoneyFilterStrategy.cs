using KutokAccounting.Components.Pages.Transactions.Models;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Transactions.Models;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Transactions.F;

public class MoneyFilterStrategy : IFilterStrategy
{
	public void Apply(Filters filters, IFilterDefinition<TransactionView> filterDefinition)
	{
		if (long.TryParse(filterDefinition?.Value?.ToString(), out long value))
		{
			Money money = new(value);

			switch (filterDefinition.Operator)
			{
				case TransactionFiltersConstants.LessThanOperator:
					filters.LessThan = money;

					break;
				case TransactionFiltersConstants.MoreThanOperator:
					filters.MoreThan = money;

					break;
				default:
					filters.Value = money;

					break;
			}
		}
	}
}