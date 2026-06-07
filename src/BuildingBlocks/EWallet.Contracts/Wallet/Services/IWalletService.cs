namespace EWallet.Contracts;

public interface IWalletService
{
    Task CreateWalletAsync(CreateWalletRequest model);
}