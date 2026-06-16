using EWallet.Contracts;
using EWallet.Wallet.DTOs;

namespace EWallet.Wallet.Services;

public interface IWalletService : IWalletClient
{
    Task<WalletDTO?> GetWalletAsync(Guid userId, CancellationToken cancellationToken = default);
}