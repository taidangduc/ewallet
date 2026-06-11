using EWallet.Contracts;

namespace EWallet.Identity.Services;

//ref: https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
public class WalletClient : IWalletClient
{
    private readonly HttpClient _httpClient;

    public WalletClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CreateWalletAsync(CreateWalletRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/wallet", request);
        response.EnsureSuccessStatusCode();
    }
}