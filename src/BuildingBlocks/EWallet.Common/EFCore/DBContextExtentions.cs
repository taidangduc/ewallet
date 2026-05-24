using Microsoft.EntityFrameworkCore;
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
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{connectionString}' is not found.");
        }

        builder.Services.AddDbContext<TDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        action?.Invoke(builder);
    }
}