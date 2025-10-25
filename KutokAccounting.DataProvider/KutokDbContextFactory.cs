using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KutokAccounting.DataProvider;

public class KutokDbContextFactory : IDesignTimeDbContextFactory<KutokDbContext>
{
	public KutokDbContext CreateDbContext(string[] args)
	{
		DbContextOptionsBuilder<KutokDbContext> optionsBuilder = new();

		optionsBuilder.UseSqlite(KutokConfigurations.ConnectionString);

		return new KutokDbContext(optionsBuilder.Options);
	}
}