using System.Security.Claims;
using EWallet.Contracts;
using EWallet.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Identity.Data;

public class IdentityDbContextSeed
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IWalletService _walletService;
    private readonly IEnumerable<User> _users =
    [
        new()
        {
            Id = Guid.NewGuid(),
            UserName = "admin",
            Email = "admin@example.com",
            SecurityStamp = Guid.NewGuid().ToString(),
        },
        new()
        {
            Id = Guid.NewGuid(),
            UserName = "user",
            Email = "user@example.com",
            SecurityStamp = Guid.NewGuid().ToString(),
        }
    ];

    public IdentityDbContextSeed(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, IWalletService walletService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _walletService = walletService;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedUsersAsync();
    }

    public async Task SeedRolesAsync()
    {
        if (!await _roleManager.RoleExistsAsync(Authorization.Roles.Admin))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = Authorization.Roles.Admin });
        }
        if (!await _roleManager.RoleExistsAsync(Authorization.Roles.User))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = Authorization.Roles.User });
        }
    }

    public async Task SeedUsersAsync()
    {
        if (!await _userManager.Users.AnyAsync())
        {
            if (await _userManager.FindByNameAsync(_users.First().UserName) == null)
            {
                var result = await _userManager.CreateAsync(_users.First(), "Admin@123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(_users.First(), Authorization.Roles.Admin);
                    await _userManager.AddClaimsAsync(_users.First(), new[]
                    {
                        new Claim("scope", Authorization.Permissions.All)
                    });
                    await _walletService.CreateWalletAsync(new CreateWalletRequest
                    {
                        UserId = _users.First().Id,
                    });
                }
            }

            if (await _userManager.FindByNameAsync(_users.Last().UserName) == null)
            {
                var result = await _userManager.CreateAsync(_users.Last(), "User@123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(_users.Last(), Authorization.Roles.User);
                    await _userManager.AddClaimsAsync(_users.Last(), new[]
                    {
                        new Claim("scope", Authorization.Permissions.Read),
                        new Claim("scope", Authorization.Permissions.Write),
                    });
                    await _walletService.CreateWalletAsync(new CreateWalletRequest
                    {
                        UserId = _users.Last().Id,
                    });
                }
            }
        }
    }
}