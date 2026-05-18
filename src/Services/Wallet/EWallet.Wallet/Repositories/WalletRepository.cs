using EWallet.Wallet.Data;

namespace EWallet.Wallet.Repositories;

public class WalletRepository : Repository<Entities.Wallet>, IWalletRepository
{
    public WalletRepository(WalletDbContext dbContext) : base(dbContext)
    {
    }
}