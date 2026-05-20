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
    options.AddPolicy("AllowOrigins", policy =>
    {
        policy.WithOrigins(appSettings.CORS.AllowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Database
services.AddDbContext<WalletDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));

// Configure services
services.AddScoped<IUnitOfWork, DbContextUnitOfWork<WalletDbContext>>();
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowOrigins");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "EWallet API V1");
    options.RoutePrefix = "swagger";
});

app.UseExceptionHandler();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<WalletDbContext>();
    await database.Database.MigrateAsync();
}

app.Run();