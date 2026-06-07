using EWallet.Common.Exceptions;
using EWallet.Identity.DTOs;
using EWallet.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace EWallet.Identity.Services;

//ref: https://code-maze.com/swagger-authorization-aspnet-core/
public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtProvider _jwtProvider;
    public IdentityService(UserManager<User> userManager, SignInManager<User> signInManager, IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtProvider = jwtProvider;
    }

    public Task<UserDTO> GetUserAsync(Guid userId)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var roles = _userManager.GetRolesAsync(user).Result.ToList();

        var userDto = new UserDTO
        {
            Id = userId,
            Username = user.UserName,
            Roles = roles
        };

        return Task.FromResult(userDto);
    }

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
        if (!result.Succeeded)
        {
            throw new ValidationException("Invalid username or password");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var scopes = await _userManager.GetClaimsAsync(user);

        var tokenString = await _jwtProvider.GenerateTokenAsync(user, roles, scopes);

        return tokenString;
    }
}