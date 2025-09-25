using KutokAccounting.DataProvider;
using KutokAccounting.Logging;
using KutokAccounting.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KutokAccounting;

public partial class App : Application
{
	private readonly KutokDbContext _dbContext;
	private readonly ILogger<App> _logger;
	private readonly IFileLoggerProcessor _processor;
	private Task _loggingTask;

	public App(KutokDbContext dbContext,
		ILogger<App> logger,
		IFileLoggerProcessor processor)
	{
		_dbContext = dbContext;
		_logger = logger;
		_processor = processor;
	}

	protected override void OnStart()
	{
		InitializeComponent();

		_dbContext.Database.MigrateAsync().ContinueWith(t =>
		{
			if (t.IsCompletedSuccessfully)
			{
				_logger.LogInformation("Migration applied successfully.");
			}
			else
			{
				_logger.LogInformation(t.Exception, "Migration failed.");
			}
		});

		LogFilesCleaner.CleanAsync(CancellationToken.None).ContinueWith(t =>
		{
			_logger.LogInformation("Old logs was successfully cleaned.");
		});

		_loggingTask = _processor.ProcessAsync(CancellationToken.None);
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new MainPage())
		{
			Title = "Kutok"
		};
	}
}