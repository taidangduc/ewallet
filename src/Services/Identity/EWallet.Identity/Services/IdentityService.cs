using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EWallet.Common.Exceptions;
using EWallet.Identity.ConfigurationOptions;
using EWallet.Identity.Entities;
using EWallet.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EWallet.Identity.Services;

//ref: https://code-maze.com/swagger-authorization-aspnet-core/
public class IdentityService : IIdentityService
{
    private readonly IOptions<AppSettings> _appSettings;
    private readonly UserManager<User> _userManager;
    public IdentityService(IOptions<AppSettings> appSettings, UserManager<User> userManager)
    {
        _appSettings = appSettings;
        _userManager = userManager;
    }

    public async Task<AuthenticationModel> AuthenticateAsync(LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var permissions = await _userManager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        claims.AddRange(permissions);

        var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_appSettings.Value.Jwt.SecretKey));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _appSettings.Value.Jwt.Authority,
            audience: _appSettings.Value.Jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_appSettings.Value.Jwt.ExpiredTime),
            signingCredentials: signingCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        var authenticationModel = new AuthenticationModel
        {
            AccessToken = tokenString
        };

        return authenticationModel;
    }
}