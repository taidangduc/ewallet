using EWallet.Wallet.Entities;

namespace EWallet.Wallet.DTOs;

public class TransactionDTO
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedDateTime { get; set; }
}