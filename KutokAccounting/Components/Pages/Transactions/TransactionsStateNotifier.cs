namespace KutokAccounting.Components.Pages.Transactions;

public class TransactionsStateNotifier
{
	public event Func<Task>? OnTransactionsChangedAsync;
	
	public async Task TransactionsChangedAsync()
	{
		if (OnTransactionsChangedAsync is not null)
		{
			await OnTransactionsChangedAsync.Invoke();
		}
	}
}