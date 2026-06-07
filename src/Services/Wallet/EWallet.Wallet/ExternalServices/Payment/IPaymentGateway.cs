namespace EWallet.Wallet.ExternalServices.Payment;

public interface IPaymentGateway
{
    Task<PaymentResponse> ChargeAsync(PaymentRequest request);
    Task<PaymentResponse> PayoutAsync(PayoutRequest request);
}

public class PaymentRequest
{
    public string CardId { get; set; }
}

public class PayoutRequest
{
    public string CardId { get; set; }
}

public class PaymentResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}