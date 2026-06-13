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

builder.Services.AddCustomJwtAuthentication(appSettings.Jwt);

builder.Services.AddAuthorization();

// Ocelot
builder.Configuration
.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseCors(appSettings.CORS.AllowAnyOrigin ? "AllowOrigin" : "AllowOrigins");

app.UseAuthentication();

app.UseAuthorization();

await app.UseOcelot();

app.Run();