using EWallet.Common.Web;
using EWallet.Gateways.WebAPI.ConfigurationOptions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

//ref: https://code-maze.com/aspnetcore-api-gateway-with-ocelot

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var appSettings = new AppSettings();
configuration.Bind(appSettings);
services.Configure<AppSettings>(configuration);

// Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

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
builder.Services.AddCustomJwtAuthentication(appSettings.Jwt);

var app = builder.Build();

app.UseCors(appSettings.CORS.AllowAnyOrigin ? "AllowOrigin" : "AllowOrigins");

await app.UseOcelot();

app.UseAuthentication();
app.UseAuthorization();

app.Run();