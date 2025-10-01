using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.Stores.Models;

public class StoreSearchParameters
{
	public string? Address { get; set; }
	public string? Name { get; set; }
	public bool? IsOpened { get; set; }
	public DateTime? SetupDate { get; set; }
	public Pagination Pagination { get; set; }
}