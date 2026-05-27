using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EWallet.Identity.ConfigurationOptions;
using EWallet.Identity.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EWallet.Identity.Services;

public class JwtProvider : IJwtProvider
{
    private readonly AppSettings _appSettings;
    public JwtProvider(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public Task<string> GenerateTokenAsync(User user, IList<string> roles, IList<Claim> scopes)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));
        claims.AddRange(scopes);

        var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_appSettings.Jwt.SecretKey));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: _appSettings.Jwt.Authority,
            audience: _appSettings.Jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_appSettings.Jwt.ExpiredTime),
            signingCredentials: signingCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return Task.FromResult(tokenString);
    }
}