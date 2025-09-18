namespace KutokAccounting.Services.Stores.Models;

public class SearchParameters
{
	public string? Address { get; set; }
	public string? Name { get; set; }
	public bool? IsOpened { get; set; }
	public DateTime? SetupDate { get; set; }
}