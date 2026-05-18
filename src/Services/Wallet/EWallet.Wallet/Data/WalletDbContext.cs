using System.Reflection;
using EWallet.Common.EFCore;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Wallet.Data;

public class WalletDbContext : DbContextUnitOfWork<WalletDbContext>
{
    public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
    {
    }

    public DbSet<Entities.Transaction> Transactions => Set<Entities.Transaction>();
    public DbSet<Entities.Wallet> Wallets => Set<Entities.Wallet>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}