using Microsoft.EntityFrameworkCore.Storage.Json;

namespace KutokAccounting;

public static class KutokConfigurations
{
    public static readonly string ConnectionString =
        $"Data Source={Path.Combine(AppContext.BaseDirectory, "KutokData.db")}";

    public const string WriteOperationsSemaphore = "WriteOperationsSemaphore";

    public const string LogOperationsSemaphore = "LogOperationsSemaphore";

    
    
}