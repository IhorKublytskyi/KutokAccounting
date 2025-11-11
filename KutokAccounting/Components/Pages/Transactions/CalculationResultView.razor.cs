using KutokAccounting.DataProvider.Models;
using Microsoft.AspNetCore.Components;

namespace KutokAccounting.Components.Pages.Transactions;

public partial class CalculationResultView : ComponentBase
{
	[Parameter]
	public CalculationResult Value { get; set; }
}