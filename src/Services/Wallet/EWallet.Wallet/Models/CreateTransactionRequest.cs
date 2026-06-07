using EWallet.Wallet.Entities;

namespace EWallet.Wallet.Models;

public class CreateTransactionRequest
{
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string CardId { get; set; }
}