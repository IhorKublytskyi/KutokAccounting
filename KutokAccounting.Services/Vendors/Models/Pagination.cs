namespace KutokAccounting.Services.Vendors.DataTransferObjects;

public record struct Pagination
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Skip => (Page - 1) * PageSize;
}
