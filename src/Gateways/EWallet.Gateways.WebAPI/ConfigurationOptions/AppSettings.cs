using static EWallet.Common.Web.JwtAuthentication;

namespace EWallet.Gateways.WebAPI.ConfigurationOptions;

public class AppSettings
{
    public CORS CORS { get; set; }
    public JwtOptions Jwt { get; set; }
}