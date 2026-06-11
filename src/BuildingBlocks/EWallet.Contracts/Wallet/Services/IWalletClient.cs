namespace EWallet.Contracts;

public interface IWalletClient
{
    Task CreateWalletAsync(CreateWalletRequest model);
}