using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EWallet.Identity.ConfigurationOptions;
using EWallet.Identity.Entities;
using EWallet.Identity.Services;
using Microsoft.Extensions.Options;
using Moq;
using static EWallet.Common.Web.JwtAuthentication;

namespace EWallet.Identity.UnitTests.Services;

public class JwtProviderTests
{
    private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
    private readonly JwtProvider _jwtProvider;

    public JwtProviderTests()
    {
        _appSettingsMock = new Mock<IOptions<AppSettings>>();

        _jwtProvider = new JwtProvider(_appSettingsMock.Object);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwt()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "peter"
        };
        var roles = new List<string> { "User" };
        var scopes = new List<Claim>
        {
            new Claim("scope", "read"),
            new Claim("scope", "write")
        };

        _appSettingsMock.Setup(x => x.Value).Returns(new AppSettings
        {
            Jwt = new JwtOptions
            {
                SecretKey = "a-string-secret-at-least-256-bits-long",
                Authority = "fake-authority",
                Audience = "fake-audience",
                ExpiredTime = 60
            }
        });

        // Act
        var result = new JwtProvider(_appSettingsMock.Object).GenerateTokenAsync(user, roles, scopes);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(result.Result);

        Assert.NotNull(token);
        Assert.Equal(user.UserName, token.Claims.First(x => x.Type == ClaimTypes.Name).Value);
        Assert.Equal(user.Id.ToString(), token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        Assert.Contains(token.Claims, x => x.Type == ClaimTypes.Role && x.Value == "User");
        Assert.Contains(token.Claims, x => x.Type == "scope" && x.Value == "read");
        Assert.Contains(token.Claims, x => x.Type == "scope" && x.Value == "write");
    }

    [Fact]
    public async Task GenerateToken_WhenAppSettingsIsNull_ShouldThrowException()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "peter"
        };
        var roles = new List<string> { "User" };
        var scopes = new List<Claim>
        {
            new Claim("scope", "read"),
            new Claim("scope", "write")
        };

        _appSettingsMock.Setup(x => x.Value).Returns((AppSettings)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _jwtProvider.GenerateTokenAsync(user, roles, scopes));
    }

    // NOTE: 
    // When the secret key is invalid (less than 256 bits, etc.),
    // the SymmetricSecurityKey constructor will throw an exception.
    [Fact]
    public async Task GenerateToken_WhenSecretKeyIsInvalid_ShouldThrowException()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "peter"
        };
        var roles = new List<string> { "User" };
        var scopes = new List<Claim>
        {
            new Claim("scope", "read"),
            new Claim("scope", "write")
        };

        _appSettingsMock.Setup(x => x.Value).Returns(new AppSettings
        {
            Jwt = new JwtOptions
            {
                SecretKey = "short-key",
                Authority = "fake-authority",
                Audience = "fake-audience",
                ExpiredTime = 60
            }
        });

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _jwtProvider.GenerateTokenAsync(user, roles, scopes));
    }
}