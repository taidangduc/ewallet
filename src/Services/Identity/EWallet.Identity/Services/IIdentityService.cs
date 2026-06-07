using EWallet.Identity.DTOs;

namespace EWallet.Identity.Services;

public interface IIdentityService
{
    Task<UserDTO> GetUserAsync(Guid userId);
    Task<string> AuthenticateAsync(string username, string password);
}