using KutokAccounting.Services.Stores.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace KutokAccounting.Components.Pages.Stores;

public partial class MoreStoreInfoDialog
{
	[CascadingParameter]
	public IMudDialogInstance MudDialog { get; set; }

	[Parameter]
	public StoreDto Store { get; set; } = null!;

	private void Cancel(MouseEventArgs obj)
	{
		MudDialog.Cancel();
	}
}