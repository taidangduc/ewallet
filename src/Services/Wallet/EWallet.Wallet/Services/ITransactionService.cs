using EWallet.Wallet.DTOs;
using EWallet.Wallet.Entities;

namespace EWallet.Wallet.Services;

public interface ITransactionService
{
    Task<List<TransactionDTO>> GetTransactionsAsync(Guid userId);
    Task CreateTransactionAsync(CreateTransactionModel model);
}

public class CreateTransactionModel
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string CardId { get; set; }
}