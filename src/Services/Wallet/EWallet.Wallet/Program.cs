using EWallet.Common.Core;
using EWallet.Common.EFCore;
using EWallet.Common.Web;
using EWallet.Contracts;
using EWallet.Wallet.ConfigurationOptions;
using EWallet.Wallet.Data;
using EWallet.Wallet.Entities;
using EWallet.Wallet.ExternalServices.Payment;
using EWallet.Wallet.Repositories;
using EWallet.Wallet.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var appSettings = new AppSettings();
configuration.Bind(appSettings);

services.Configure<AppSettings>(configuration);

// CORS
services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins", builder => builder
        .WithOrigins(appSettings.CORS.AllowedOrigins)
        .AllowAnyMethod()
        .AllowAnyHeader());

    options.AddPolicy("AllowOrigin", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

// Authentication
services.AddCustomJwtAuthentication(appSettings.Jwt);
services.AddAuthorization();

// Database
builder.AddCustomDbContext<WalletDbContext>(appSettings.ConnectionStrings.DefaultConnection);

// Configure DI
services.AddScoped<IUnitOfWork, WalletDbContext>();
services.AddScoped<ITransactionRepository, TransactionRepository>();
services.AddScoped<IWalletRepository, WalletRepository>();
services.AddScoped<IRepository<Wallet>, Repository<Wallet>>();
services.AddScoped<IRepository<Transaction>, Repository<Transaction>>();
services.AddScoped<WalletService>();
services.AddScoped<IWalletService, WalletService>();
services.AddScoped<ITransactionService, TransactionService>();
services.AddScoped<IPaymentGateway, PaymentGateway>();

services.AddHttpContextAccessor();
services.AddScoped<ICurrentWebUser, CurrentWebUser>();

services.AddExceptionHandler<GlobalExceptionHandler>();
services.AddProblemDetails();

builder.Services.AddControllers();

services.AddSwaggerDocumentation("EWallet", "v1");

var app = builder.Build();

app.UseCors(appSettings.CORS.AllowAnyOrigin ? "AllowOrigin" : "AllowOrigins");

app.UseSwaggerDocumentation("EWallet", "v1");

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Migrate database
using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<WalletDbContext>();
    await database.Database.MigrateAsync();
}

app.Run();