using EWallet.Identity.Models;

namespace EWallet.Identity.Services;

public interface IIdentityService
{
    Task<AuthenticationModel> AuthenticateAsync(LoginModel model);
}