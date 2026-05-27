using System.Security.Claims;
using EWallet.Identity.Entities;

namespace EWallet.Identity.Services;

public interface IJwtProvider
{
    Task<string> GenerateTokenAsync(User user, IList<string> roles, IList<Claim> claims);
}