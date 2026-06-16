namespace EWallet.Wallet.Entities;

public class Wallet
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }
    public DateTimeOffset CreatedDateTime { get; set; }
    public DateTimeOffset? UpdatedDateTime { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}