namespace EWallet.Wallet.ExternalServices.Payment;

public class PaymentGateway : IPaymentGateway
{
    private readonly List<TestCard> _testCards =
    [
        new()
        {
            Id = "token_98765abcdef",
            CardNumber = "4242424242424242",
            Status = PaymentStatus.Success
        },
        new()
        {
            Id = "token_12345abcdef",
            CardNumber = "4000000000000002",
            Status = PaymentStatus.Failed
        }
    ];

    // For testing purpose only
    public async Task<PaymentResponse> ChargeAsync(PaymentRequest request)
    {
        await Task.Delay(500);
        return ProcessPayment(request.CardNumber);
    }

    public async Task<PaymentResponse> PayoutAsync(PayoutRequest request)
    {
        await Task.Delay(500);
        return ProcessPayment(request.CardNumber);
    }

    private PaymentResponse ProcessPayment(string cardNumber)
    {
        var testCard = _testCards.FirstOrDefault(c => c.CardNumber == cardNumber);

        if (testCard == null)
        {
            return new PaymentResponse
            {
                Success = false,
                Message = "Card not found"
            };
        }

        return testCard.Status switch
        {
            PaymentStatus.Success => new PaymentResponse
            {
                Success = true,
                Message = "Success"
            },
            PaymentStatus.Failed => new PaymentResponse
            {
                Success = false,
                Message = "Failed"
            },
            _ => new PaymentResponse
            {
                Success = false,
                Message = "Unknown"
            }
        };
    }
}