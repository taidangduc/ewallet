
using static EWallet.Common.Web.JwtAuthentication;

namespace EWallet.Identity.ConfigurationOptions;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public ServiceOptions Services { get; set; }
    public JwtOptions Jwt { get; set; }
}