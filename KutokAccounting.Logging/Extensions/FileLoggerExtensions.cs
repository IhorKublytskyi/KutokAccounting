using KutokAccounting.Logging.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Logging.Extensions;

public static class FileLoggerExtensions
{
	public static IServiceCollection AddFileLogging(this IServiceCollection services)
	{
		services.AddSingleton<IFileLogChannelHolder, FileLogChannelHolder>();
		services.AddSingleton<IFileLoggerProcessor, FileLoggerProcessor>();
		services.AddSingleton<ILoggerProvider, FileLoggerProvider>();

		return services;
	}
}