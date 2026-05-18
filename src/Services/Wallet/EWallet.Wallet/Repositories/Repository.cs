using EWallet.Common.EFCore;
using EWallet.Wallet.Data;

namespace EWallet.Wallet.Repositories;

public class Repository<T> : DbContextRepository<WalletDbContext, T>
    where T : class
{
    public Repository(WalletDbContext dbContext) : base(dbContext)
    {
    }
}