using KutokAccounting.DataProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KutokAccounting;

public partial class App : Application
{
	private readonly KutokDbContext _dbContext;
	private readonly ILogger<App> _logger;

	public App(KutokDbContext dbContext, ILogger<App> logger)
	{
		_dbContext = dbContext;
		_logger = logger;
	}

	protected override void OnStart()
	{
		InitializeComponent();

		_dbContext.Database.MigrateAsync().ContinueWith(t =>
		{
			if (t.IsCompletedSuccessfully)
			{
				_logger.LogInformation("Migration applied successfully");
			}
			else
			{
				_logger.LogInformation(t.Exception, "Migration failed");
			}
		});
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new MainPage())
		{
			Title = "Kutok"
		};
	}
}