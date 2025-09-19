namespace KutokAccounting.Services.Vendors.Models;

public sealed record VendorDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}