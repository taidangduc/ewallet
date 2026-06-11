using EWallet.Common.Exceptions;
using EWallet.Contracts;
using EWallet.Wallet.DTOs;
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
    public async Task CreateWalletAsync(CreateWalletRequest model)
    {
        var existingWallet = await _walletRepository.GetQueryable().AnyAsync(x => x.UserId == model.UserId);
        if (existingWallet)
        {
            throw new ValidationException($"Wallet for user {model.UserId} already exists.");
        }

        // Set initial wallet balance (for testing purposes), you can change it as needed.
        var wallet = new Entities.Wallet
        {
            Id = Guid.NewGuid(),
            UserId = model.UserId,
            Balance = 1000000,
            Currency = "USD",
            CreatedDateTime = DateTimeOffset.UtcNow
        };

        await _walletRepository.AddAsync(wallet);
        await _walletRepository.UnitOfWork.SaveChangesAsync();
    }

    public Task<WalletDTO?> GetWalletAsync(Guid userId)
    {
        return _walletRepository.GetQueryable().Select(x => new WalletDTO
        {
            Id = x.Id,
            UserId = x.UserId,
            Balance = x.Balance,
            Currency = x.Currency,
        }).FirstOrDefaultAsync(x => x.UserId == userId);
    }
}