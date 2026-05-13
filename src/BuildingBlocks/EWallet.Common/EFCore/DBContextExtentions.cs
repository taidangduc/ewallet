using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace EWallet.Common.EFCore;

public static class DBContextExtensions
{
    public static void AddCustomDbContext<TDbContext>(
        this IHostApplicationBuilder builder,
        string connectionString,
        Action<IHostApplicationBuilder>? action = null
    ) where TDbContext : DbContext
    {
        builder.Services.AddDbContext<TDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString(connectionString));
        });

        action?.Invoke(builder);
    }
}