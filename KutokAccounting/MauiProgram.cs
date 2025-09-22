using KutokAccounting.DataProvider;
using KutokAccounting.Services.Stores;
using KutokAccounting.Services.Stores.Abstractions;
using KutokAccounting.Services.Stores.Models;
using KutokAccounting.Services.Vendors;
using KutokAccounting.Services.Vendors.Validators;
using KutokAccounting.WinUI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace KutokAccounting;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddTransient<CancellationTokenSource>(_ => new CancellationTokenSource());
        builder.Services.AddMudServices();
        builder.Services.AddMauiBlazorWebView();
        
        builder.Services.AddDbContext<KutokDbContext>(options =>
        {
            options.UseSqlite(KutokConfigurations.ConnectionString);
        });
        
        builder.Services.AddScoped<IVendorService, VendorService>();
        builder.Services.AddScoped<IVendorRepository, VendorRepository>();
        builder.Services.AddScoped<VendorDtoValidator>();
        builder.Services.AddScoped<QueryParametersValidator>();
        builder.Services.AddKeyedSingleton(KutokConfigurations.WriteOperationsSemaphore,  new SemaphoreSlim(1, 1));

        builder.Services.AddScoped<IStoreBuilder, StoreQueryBuilder>();
        builder.Services.AddScoped<PageDtoValidator>();
        builder.Services.AddScoped<StoreDtoValidator>();
        builder.Services.AddScoped<IStoresRepository, StoresRepository>();
        builder.Services.AddScoped<IStoresService, StoresService>();
        

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}