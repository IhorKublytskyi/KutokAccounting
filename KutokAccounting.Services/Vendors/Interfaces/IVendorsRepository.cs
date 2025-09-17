using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Vendors.Models;

namespace KutokAccounting.Services.Vendors;

public interface IVendorRepository
{
    ValueTask CreateAsync(Vendor vendor, CancellationToken cancellationToken);
    ValueTask<Vendor> GetByIdAsync(int id, CancellationToken cancellationToken);
    ValueTask<PagedResult<Vendor>> GetAsync(QueryParameters queryParameters, CancellationToken cancellationToken);
    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
    ValueTask UpdateAsync(Vendor vendor, CancellationToken cancellationToken);
}