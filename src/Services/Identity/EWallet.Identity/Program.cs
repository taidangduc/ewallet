using EWallet.Contracts;
using EWallet.Identity.ConfigurationOptions;
using EWallet.Identity.Data;
using EWallet.Identity.Entities;
using EWallet.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var appSettings = new AppSettings();
configuration.Bind(appSettings);

services.Configure<AppSettings>(configuration);

services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));

services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<IdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

services.AddScoped<IdentityDbContextSeed>();

// Configure HttpClient
services.AddHttpClient<IWalletService, WalletService>(options =>
{
    options.BaseAddress = new Uri(appSettings.Services.Wallet.BaseUrl);
});

var app = builder.Build();

// Configure Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "EWallet.Identity API V1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    await database.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<IdentityDbContextSeed>();
    await seeder.SeedAsync();
}

app.Run();