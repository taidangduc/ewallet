using static EWallet.Common.Web.JwtAuthentication;

namespace EWallet.Wallet.ConfigurationOptions;

public class AppSettings
{
   public ConnectionStrings ConnectionStrings { get; set; }
   public CORS CORS { get; set; }
   public JwtOptions Jwt { get; set; }
}