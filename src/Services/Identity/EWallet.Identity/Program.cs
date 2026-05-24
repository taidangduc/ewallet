using EWallet.Common.EFCore;
using EWallet.Common.Web;
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

// Database
builder.AddCustomDbContext<IdentityDbContext>(appSettings.ConnectionStrings.DefaultConnection);

// Identity
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

// OpenAPI
services.AddSwaggerDocumentation("EWallet.Identity", "v1");

builder.Services.AddControllers();

// Configure DI
services.AddScoped<IdentityDbContextSeed>();
services.AddScoped<IIdentityService, IdentityService>();

// Configure HttpClient
services.AddHttpClient<IWalletService, WalletService>(options =>
{
    options.BaseAddress = new Uri(appSettings.Services.Wallet.BaseUrl);
});

builder.Services.AddCustomJwtAuthentication(appSettings.Jwt);
services.AddAuthorization();

var app = builder.Build();

app.UseSwaggerDocumentation("EWallet.Identity", "v1");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Migrate and seed database
using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    await database.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<IdentityDbContextSeed>();
    await seeder.SeedAsync();
}

app.Run();