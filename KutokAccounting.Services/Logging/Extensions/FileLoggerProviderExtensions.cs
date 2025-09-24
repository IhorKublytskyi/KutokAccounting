using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Services.Logging.Extensions;

public static class FileLoggerProviderExtensions
{
    public static ILoggingBuilder AddProvider<T>(this ILoggingBuilder builder, Func<IServiceProvider, FileLoggerProvider> factory)
    where T : class, ILoggerProvider
    {
        builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>(factory);
        return builder;
    }
}
