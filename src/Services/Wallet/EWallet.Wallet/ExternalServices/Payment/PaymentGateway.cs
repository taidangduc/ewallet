namespace EWallet.Wallet.ExternalServices.Payment;

public class PaymentGateway : IPaymentGateway
{
    private readonly List<TestCard> _testCards =
    [
        new()
        {
            Id = "card_98765abcdef",
            Last4 = "4242",
            CardNumber = "4242424242424242",
            CardBrand = "Visa",
            Status = PaymentStatus.Success
        },
        new()
        {
            Id = "card_12345abcdef",
            Last4 = "0002",
            CardNumber = "4000000000000002",
            CardBrand = "MasterCard",
            Status = PaymentStatus.Failed
        }
    ];

    // For testing purpose only
    public async Task<PaymentResponse> ChargeAsync(PaymentRequest request)
    {
        await Task.Delay(500);
        return ProcessPayment(request.CardId);
    }

    public async Task<PaymentResponse> PayoutAsync(PayoutRequest request)
    {
        await Task.Delay(500);
        return ProcessPayment(request.CardId);
    }

    private PaymentResponse ProcessPayment(string cardId)
    {
        var testCard = _testCards.FirstOrDefault(c => c.Id == cardId);

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