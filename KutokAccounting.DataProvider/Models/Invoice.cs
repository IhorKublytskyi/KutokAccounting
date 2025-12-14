namespace KutokAccounting.DataProvider.Models;

public class Invoice
{
	public int Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public required string Number { get; set; }
	public required InvoiceStatus Status { get; set; }
	public int StoreId { get; set; }
	public Store? Store { get; set; }
	public int VendorId { get; set; }
	public Vendor? Vendor { get; set; }
	public IEnumerable<Transaction>? Transactions { get; set; }
}