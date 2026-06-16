using System.Security.Claims;
using EWallet.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Identity.Data;

public class IdentityDbContextSeed
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IEnumerable<User> _users =
    [
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            UserName = "admin",
            Email = "admin@example.com",
            SecurityStamp = Guid.NewGuid().ToString(),
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
            UserName = "user",
            Email = "user@example.com",
            SecurityStamp = Guid.NewGuid().ToString(),
        }
    ];

    public IdentityDbContextSeed(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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
                }
            }
        }
    }
}