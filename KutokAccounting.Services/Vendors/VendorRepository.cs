using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Vendors.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KutokAccounting.Services.Vendors;

public sealed class VendorRepository : IVendorRepository
{
    private readonly KutokDbContext _dbContext;
    private readonly SemaphoreSlim _semaphoreSlim;

    public VendorRepository(KutokDbContext dbContext, [FromKeyedServices(KutokConfigurations.WriteOperationsSemaphore)] SemaphoreSlim semaphoreSlim) 
    {
        _dbContext = dbContext;
        _semaphoreSlim = semaphoreSlim;
    }

    public async ValueTask CreateAsync(Vendor vendor, CancellationToken cancellationToken)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            await _dbContext.Vendors.AddAsync(vendor, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.InnerException);
        }
        finally 
        {
            _semaphoreSlim.Release();
        }
    }

    public async ValueTask<VendorPagedResult> GetAsync(QueryParameters queryParameters, CancellationToken cancellationToken)
    {

        var query = _dbContext.Vendors.AsNoTracking();

        if (string.IsNullOrWhiteSpace(queryParameters.Name) is false)
            query = query.Where(v => v.Name == queryParameters.Name);

        if (string.IsNullOrEmpty(queryParameters.SearchString) is false)
            query = query.Where(v =>
                EF.Functions.Like(v.Name, $"%{queryParameters.SearchString}%") || EF.Functions.Like(v.Description, $"%{queryParameters.SearchString}%"));

        Task<int> countTask = query.CountAsync(cancellationToken);

        var vendors = await query
            .AsNoTracking()
            .Skip(queryParameters.Pagination.Skip)
            .Take(queryParameters.Pagination.PageSize)
            .Select(v => new Vendor
            {
                Id = v.Id,
                Name = v.Name,
                Description = v.Description
            })
            .OrderBy(v => v.Name)
            .ToListAsync(cancellationToken);

        return new VendorPagedResult() 
        {
            Vendors = vendors,
            Count = await countTask,
        };
    }

    public async ValueTask<Vendor?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Vendors
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            await _dbContext.Vendors
            .Where(v => v.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async ValueTask UpdateAsync(Vendor vendor, CancellationToken cancellationToken)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            await _dbContext.Vendors
             .Where(v => v.Id == vendor.Id)
             .ExecuteUpdateAsync(v => v
                 .SetProperty(p => p.Name, vendor.Name)
                 .SetProperty(p => p.Description, vendor.Description), cancellationToken);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
}