using EWallet.Common.Exceptions;
using EWallet.Identity.Entities;
using EWallet.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace EWallet.Identity.Services;

//ref: https://code-maze.com/swagger-authorization-aspnet-core/
public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtProvider _jwtProvider;
    public IdentityService(UserManager<User> userManager, IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }

    public async Task<AuthenticationModel> AuthenticateAsync(LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var scopes = await _userManager.GetClaimsAsync(user);

        var tokenString = await _jwtProvider.GenerateTokenAsync(user, roles, scopes);
        var authenticationModel = new AuthenticationModel
        {
            AccessToken = tokenString
        };

        return authenticationModel;
    }
}