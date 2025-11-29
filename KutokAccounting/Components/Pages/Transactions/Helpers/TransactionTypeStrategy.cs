using KutokAccounting.Components.Pages.Transactions.Models;
using KutokAccounting.Services.Transactions.Models;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Transactions.Helpers;

public class TransactionTypeStrategy : IFilterStrategy
{
	public void Apply(Filters filters, IFilterDefinition<TransactionView> filterDefinition)
	{
		if (int.TryParse(filterDefinition?.Value?.ToString(), out int id))
		{
			filters.TransactionTypeId = id;
		}
	}
}