using EWallet.Wallet.Data;

namespace EWallet.Wallet.Repositories;

public class TransactionRepository : Repository<Entities.Transaction>, ITransactionRepository
{
    public TransactionRepository(WalletDbContext dbContext) : base(dbContext)
    {
    }
}