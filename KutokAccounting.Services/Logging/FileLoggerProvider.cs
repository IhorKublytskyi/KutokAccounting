using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace KutokAccounting.Services.Logging;

[ProviderAlias("FileLogger")]
public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? _onChangeToken;
    private FileLoggerConfiguration _currentConfig;
    private readonly SemaphoreSlim _semaphoreSlim;
    private readonly ConcurrentDictionary<string, FileLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
    public FileLoggerProvider([FromKeyedServices(KutokConfigurations.WriteOperationsSemaphore)] SemaphoreSlim semaphoreSlim, IOptionsMonitor<FileLoggerConfiguration> config)
    {
        _currentConfig = config.CurrentValue;
        _onChangeToken = config.OnChange(updatedConfig => _currentConfig =  updatedConfig);
        _semaphoreSlim = semaphoreSlim;
    }

    public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, name => new FileLogger(name, _semaphoreSlim, GetCurrentConfig));
    private FileLoggerConfiguration GetCurrentConfig() => _currentConfig;

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }
}
