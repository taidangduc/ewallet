using EWallet.Wallet.Entities;

namespace EWallet.Wallet.Models;

public class TransactionModel
{
    public Guid WalletId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string CardNumber { get; set; }
}