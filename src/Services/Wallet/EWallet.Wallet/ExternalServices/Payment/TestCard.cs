namespace EWallet.Wallet.ExternalServices.Payment;

// NOTE: For testing purpose only
public class TestCard
{
    public string Id { get; set; }
    public string Last4 { get; set; }
    public string CardNumber { get; set; }
    public PaymentStatus Status { get; set; }
}

public enum PaymentStatus
{
    Success,
    Failed
}