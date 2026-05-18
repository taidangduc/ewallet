namespace EWallet.Contracts;

public interface IWalletService
{
    Task CreateWalletAsync(CreateWalletModel model);
}