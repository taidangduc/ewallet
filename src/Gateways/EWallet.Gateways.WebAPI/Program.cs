using System.Security.Claims;
using System.Threading.RateLimiting;
using EWallet.Common.Web;
using EWallet.Gateways.WebAPI.ConfigurationOptions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;

//ref: https://code-maze.com/aspnetcore-api-gateway-with-ocelot
//ref: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/yarp/config-files?view=aspnetcore-10.0
//ref: https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-10.0
//ref: https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-10.0
//ref: https://www.c-sharpcorner.com/article/rate-limiting-and-throttling-in-asp-net-core-web-api/

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

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Too many requests.", cancellationToken);
    };

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        // You can bind rate limits to users, API keys, or IPs.
        // NOTE: use ForwardedHeaders middleware if you are behind a reverse proxy.
        var partitionKey =
            httpContext.User?.FindFirst("sub")?.Value ??
            httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            "anonymous";

        return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 50,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
        });
    });
});

builder.Services.AddHealthChecks();

builder.Services.AddReverseProxy()
    .LoadFromConfig(configuration.GetSection("Yarp"));

var app = builder.Build();

app.UseCors(appSettings.CORS.AllowAnyOrigin ? "AllowOrigin" : "AllowOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapHealthChecks("/health");

app.MapReverseProxy();

app.Run();