using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KutokAccounting.Services.Logging;

public sealed class FileLogger : ILogger
{
    private string _categoryName;

    private SemaphoreSlim _semaphoreSlim;

    private Func<FileLoggerConfiguration> _getCurrentConfig;

    public FileLogger(
        string categoryName, 
        [FromKeyedServices(KutokConfigurations.WriteOperationsSemaphore)] SemaphoreSlim semaphoreSlim,
        Func<FileLoggerConfiguration> getCurrentConfig)
    {
        _categoryName = categoryName.Split('.').Last();
        _semaphoreSlim = semaphoreSlim;
        _getCurrentConfig = getCurrentConfig;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel) =>
        _getCurrentConfig().LogLevelToString.TryGetValue(logLevel, out string? _);
    
    public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if(IsEnabled(logLevel) is false)
        {
            return;
        }

        FileLoggerConfiguration config = _getCurrentConfig();

        if(config.EventId == 0 || config.EventId == eventId.Id)
        {
            string message = formatter(state, exception);

            string log = string.Format(config.LogTemplate, eventId.Id, _categoryName, $"{DateTime.Now:HH:mm:ss:ff}", config.LogLevelToString[logLevel], message);

            await WriteLogToFileAsync(log, CancellationToken.None);
        }  
    }

    private async ValueTask WriteLogToFileAsync(string log, CancellationToken cancellationToken)
    { 
        await _semaphoreSlim.WaitAsync();

        try
        {
            DirectoryInfo directory = Directory.CreateDirectory(_getCurrentConfig().LoggingDirectoryPath);

            await File.AppendAllTextAsync(Path.Combine(directory.FullName, _getCurrentConfig().LoggingFilePath), log + Environment.NewLine, cancellationToken);
        }
        catch(IOException e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
}
