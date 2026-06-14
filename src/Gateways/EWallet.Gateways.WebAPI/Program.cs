using EWallet.Common.Web;
using EWallet.Gateways.WebAPI.ConfigurationOptions;

//ref: https://code-maze.com/aspnetcore-api-gateway-with-ocelot
//ref: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/yarp/config-files?view=aspnetcore-10.0

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

builder.Services.AddReverseProxy()
    .LoadFromConfig(configuration.GetSection("Yarp"));

var app = builder.Build();

app.UseCors(appSettings.CORS.AllowAnyOrigin ? "AllowOrigin" : "AllowOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapReverseProxy();

app.Run();