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

builder.Services.AddCustomJwtAuthentication(appSettings.Jwt);

services.AddAuthorization();

builder.Services.AddControllers();

services.AddSwaggerDocumentation("EWallet.Identity", "v1");

services.AddExceptionHandler<GlobalExceptionHandler>();
services.AddProblemDetails();

// Configure DI
services.AddHttpContextAccessor();
services.AddScoped<ICurrentWebUser, CurrentWebUser>();
services.AddScoped<IdentityDbContextSeed>();
services.AddScoped<IIdentityService, IdentityService>();
services.AddScoped<IJwtProvider, JwtProvider>();

// Configure HttpClient
services.AddHttpClient<IWalletClient, WalletClient>(options =>
{
    options.BaseAddress = new Uri(appSettings.Services.Wallet.BaseUrl);
});

var app = builder.Build();

app.UseSwaggerDocumentation("EWallet.Identity", "v1");

app.UseExceptionHandler();

//app.UseHttpsRedirection();

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