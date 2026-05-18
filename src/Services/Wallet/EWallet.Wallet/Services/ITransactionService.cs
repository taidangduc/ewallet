using EWallet.Wallet.Models;

namespace EWallet.Wallet.Services;

public interface ITransactionService
{
    Task CreateTransactionAsync(TransactionModel model);
}