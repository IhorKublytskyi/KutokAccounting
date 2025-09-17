namespace KutokAccounting.Services.Vendors.Models;

public sealed record VendorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}