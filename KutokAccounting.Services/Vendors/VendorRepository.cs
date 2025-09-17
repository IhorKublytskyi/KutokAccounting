using KutokAccounting.DataProvider;
using KutokAccounting.DataProvider.Models;
using KutokAccounting.Services.Vendors.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Services.Vendors;

public sealed class VendorRepository : IVendorRepository
{
    private readonly KutokDbContext _dbContext;
    private readonly SemaphoreSlim _semaphoreSlim;
    private readonly ILogger<VendorRepository> _logger;

    public VendorRepository(KutokDbContext dbContext, [FromKeyedServices(KutokConfigurations.WriteOperationsSemaphore)] SemaphoreSlim semaphoreSlim, ILogger<VendorRepository> logger) 
    {
        _dbContext = dbContext;
        _semaphoreSlim = semaphoreSlim;
        _logger = logger;
    }

    public async ValueTask CreateAsync(Vendor vendor, CancellationToken cancellationToken)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            await _dbContext.Vendors.AddAsync(vendor, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch(Exception e)
        {
            _logger.LogInformation(e, "Failed to create vendor with Name: {VendorName}", vendor.Name);
        }
        finally 
        {
            _semaphoreSlim.Release();
        }
    }

    public async ValueTask<PagedResult<Vendor>> GetAsync(QueryParameters queryParameters, CancellationToken cancellationToken)
    {
        var query = _dbContext.Vendors.AsNoTracking();
 
        if (string.IsNullOrWhiteSpace(queryParameters.Name) is false)
            query = query.Where(v => v.Name == queryParameters.Name);

        if (string.IsNullOrEmpty(queryParameters.SearchString) is false)
            query = query.Where(v =>
                EF.Functions.Like(v.Name, $"%{queryParameters.SearchString}%") || EF.Functions.Like(v.Description, $"%{queryParameters.SearchString}%"));
        try
        {
            Task<int> countTask = query.CountAsync(cancellationToken);

            var vendors = await query
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

            return new PagedResult<Vendor>()
            {
                Items = vendors,
                Count = await countTask,
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to retrieve vendors with QueryParameters: {QueryParameters}", queryParameters);

            throw;
        }
    }

    public async ValueTask<Vendor?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var vendor = await _dbContext.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

            return vendor;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to retrieve vendor with Id: {VendorId}", id);

            throw;
        }
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
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete vendor with Id: {VendorId}", id);
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
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to update vendor with Id: {VendorId}, Name: {VendorName}", vendor.Id, vendor.Name);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
}