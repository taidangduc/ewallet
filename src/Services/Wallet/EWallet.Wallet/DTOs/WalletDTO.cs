namespace EWallet.Wallet.Controllers;

public class WalletDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }
}