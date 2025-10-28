namespace KutokAccounting.Services.Stores.Dtos;

public class StoreDto
{
	public int Id { get; set; } = -1;
	public string Name { get; set; }
	public bool IsOpened { get; set; }
	public DateTime SetupDate { get; set; }
	public string? Address { get; set; }
}