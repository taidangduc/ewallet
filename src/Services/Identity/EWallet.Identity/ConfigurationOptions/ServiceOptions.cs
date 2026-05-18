namespace EWallet.Identity.ConfigurationOptions;

public class ServiceOptions
{
    public WalletServiceOptions Wallet { get; set; }
}

public class WalletServiceOptions
{
    public string BaseUrl { get; set; }
}