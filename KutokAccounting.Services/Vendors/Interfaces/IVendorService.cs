using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Vendors.Models;

namespace KutokAccounting.Services.Vendors;

public interface IVendorService
{
    ValueTask<Vendor> CreateAsync(VendorDto request, CancellationToken cancellationToken);
    ValueTask<Vendor> GetByIdAsync(int id, CancellationToken cancellationToken);
    ValueTask<PagedResult<Vendor>> GetAsync(VendorQueryParameters vendorQueryParameters, CancellationToken cancellationToken);
    ValueTask UpdateAsync(VendorDto request, CancellationToken cancellationToken);
    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
}