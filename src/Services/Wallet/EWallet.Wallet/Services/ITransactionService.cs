using EWallet.Wallet.DTOs;
using EWallet.Wallet.Models;

namespace EWallet.Wallet.Services;

public interface ITransactionService
{
    Task<List<TransactionDTO>> GetTransactionsAsync(Guid walletId);
    Task CreateTransactionAsync(TransactionModel model);
}