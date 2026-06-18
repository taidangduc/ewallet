using System.Text.RegularExpressions;
using System.Security.Claims;
using EWallet.Common.Exceptions;
using EWallet.Contracts;
using EWallet.Identity.DTOs;
using EWallet.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Identity.Services;

//ref: https://code-maze.com/swagger-authorization-aspnet-core/
public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IWalletClient _walletClient;
    public IdentityService(UserManager<User> userManager, SignInManager<User> signInManager, IJwtProvider jwtProvider, IWalletClient walletClient)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtProvider = jwtProvider;
        _walletClient = walletClient;
    }

    public async Task<UserDTO> GetUserAsync(Guid userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var userDto = new UserDTO
        {
            Id = user.Id,
            Username = user.UserName,
            Roles = (await _userManager.GetRolesAsync(user)).ToList()
        };

        return userDto;
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

    public async Task CreateUserAsync(string username, string email, string password)
    {
        if (await _userManager.FindByNameAsync(username) != null)
        {
            throw new ValidationException("Username already exists");
        }

        if (await _userManager.FindByEmailAsync(email) != null)
        {
            throw new ValidationException("Email already exists");
        }

        var user = new User
        {
            UserName = username,
            Email = email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await _userManager.AddToRoleAsync(user, Authorization.Roles.User);
        await _userManager.AddClaimsAsync(user, new[]
        {
            new Claim("scope", Authorization.Permissions.Read),
            new Claim("scope", Authorization.Permissions.Write),
        });

        try
        {
            await _walletClient.CreateWalletAsync(new CreateWalletRequest
            {
                UserId = user.Id,
            });
        }
        catch (Exception ex)
        {
            await _userManager.DeleteAsync(user);
            throw new Exception("Occur error when creating wallet", ex);
        }
    }
}