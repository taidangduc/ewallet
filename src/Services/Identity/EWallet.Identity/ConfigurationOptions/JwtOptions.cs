namespace EWallet.Identity.ConfigurationOptions;

public class JwtOptions
{
    public string SecretKey { get; set; }
    public string Authority { get; set; }
    public string Audience { get; set; }
    public int ExpiredTime { get; set; }
}