using FluentValidation;
using KutokAccounting.DataProvider;
using KutokAccounting.Services.TransactionTypes;
using KutokAccounting.Services.TransactionTypes.Interfaces;
using KutokAccounting.Services.TransactionTypes.Models;
using KutokAccounting.Services.TransactionTypes.Validators;
using KutokAccounting.Services.Vendors;
using KutokAccounting.Services.Vendors.Models;
using KutokAccounting.Services.Vendors.Validators;
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

        builder.Services.AddMudServices();
        builder.Services.AddMauiBlazorWebView();

        builder.Services.AddDbContext<KutokDbContext>(options =>
        {
            options.UseSqlite(KutokConfigurations.ConnectionString);
        });

        builder.Services.AddScoped<IVendorService, VendorService>();
        builder.Services.AddScoped<IVendorRepository, VendorRepository>();
        builder.Services.AddScoped<IValidator<TransactionTypeDto>, TransactionTypeDtoValidator>();
        builder.Services.AddScoped<IValidator<TransactionTypeQueryParameters>, TransactionTypeQueryParametersValidator>();
        builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
        builder.Services.AddScoped<ITransactionTypeRepository, TransactionTypeRepository>();
        builder.Services.AddScoped<IValidator<VendorDto>, VendorDtoValidator>();
        builder.Services.AddScoped<IValidator<VendorQueryParameters>, VendorQueryParametersValidator>();
        builder.Services.AddKeyedSingleton(KutokConfigurations.WriteOperationsSemaphore,  new SemaphoreSlim(1, 1));
        
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}