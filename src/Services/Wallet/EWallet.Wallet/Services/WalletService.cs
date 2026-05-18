using EWallet.Contracts;
using EWallet.Wallet.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Wallet.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;

    public WalletService(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    // Note:
    // Wallet create while creating user, called by Identity Service.
    // Here I set the default currency to "USD", you can change it.
    public async Task CreateWalletAsync(CreateWalletModel model)
    {
        var wallet = new Entities.Wallet
        {
            Id = Guid.NewGuid(),
            UserId = model.UserId,
            Balance = 0,
            Currency = "USD",
            CreatedDateTime = DateTimeOffset.UtcNow
        };

        await _walletRepository.AddAsync(wallet);
        await _walletRepository.UnitOfWork.SaveChangesAsync();
    }

    public Task<Entities.Wallet?> GetWalletAsync(Guid userId)
    {
        return _walletRepository.GetQueryable().FirstOrDefaultAsync(x => x.UserId == userId);
    }
}