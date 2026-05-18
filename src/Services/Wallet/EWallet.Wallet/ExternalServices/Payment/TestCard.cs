namespace EWallet.Wallet.ExternalServices.Payment;

// NOTE: For testing purpose only
public class TestCard
{
    public string CardNumber { get; set; }
    public PaymentStatus Status { get; set; }
}

public enum PaymentStatus
{
    Success,
    Failed
}