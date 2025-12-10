using KutokAccounting.Components.Pages.Transactions.Models;
using KutokAccounting.Services.Transactions.Models;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Transactions.Helpers;

public interface IFilterStrategy
{
	void Apply(Filters filters, IFilterDefinition<TransactionView> filterDefinition);
}