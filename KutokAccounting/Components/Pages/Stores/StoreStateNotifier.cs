namespace KutokAccounting.Components.Pages.Stores;

public class StoreStateNotifier
{
	public Func<Task>? OnStoreStateChangedAsync { get; set; }

	public void StoreStateChanged()
	{
		OnStoreStateChangedAsync?.Invoke();
	}
}