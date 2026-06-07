using System.Security.Claims;
using EWallet.Common.Exceptions;
using EWallet.Identity.Entities;
using EWallet.Identity.Models;
using EWallet.Identity.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace EWallet.Identity.UnitTests.Services;

public class IdentityServiceTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly IdentityService _identityService;

    public IdentityServiceTests()
    {
        _userManagerMock = new Mock<UserManager<User>>(
           new Mock<IUserStore<User>>().Object,
           null, null, null, null, null, null, null, null);

        _signInManagerMock = new Mock<SignInManager<User>>(
            _userManagerMock.Object,
            new Mock<HttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            null, null, null, null);

        _jwtProviderMock = new Mock<IJwtProvider>();

        _identityService = new IdentityService(_userManagerMock.Object, _signInManagerMock.Object, _jwtProviderMock.Object);
    }

    [Fact]
    public async Task AuthenticateAsync_WhenUserNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Username = "unknown",
            Password = "password"
        };

        _userManagerMock
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _identityService.AuthenticateAsync(loginRequest.Username, loginRequest.Password));
        _userManagerMock.Verify(x => x.FindByNameAsync(loginRequest.Username), Times.Once);
        _userManagerMock.Verify(x => x.GetRolesAsync(It.IsAny<User>()), Times.Never);
        _userManagerMock.Verify(x => x.GetClaimsAsync(It.IsAny<User>()), Times.Never);
        _jwtProviderMock.Verify(x => x.GenerateTokenAsync(It.IsAny<User>(), It.IsAny<List<string>>(), It.IsAny<List<Claim>>()), Times.Never);
    }

    [Fact]
    public async Task AuthenticateAsync_WhenUserIsValid_ShouldReturnLoginResponse()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "user",
        };

        var loginRequest = new LoginRequest
        {
            Username = "user",
            Password = "password"
        };

        var loginResponse = new LoginResponse
        {
            AccessToken = "fake-jwt-token"
        };

        _userManagerMock
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _signInManagerMock
            .Setup(x => x.PasswordSignInAsync(user, loginRequest.Password, false, false))
            .ReturnsAsync(SignInResult.Success);

        _userManagerMock
            .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "Admin" });

        _userManagerMock
            .Setup(x => x.GetClaimsAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<Claim> { new Claim("scope", "all") });

        var tokenString = _jwtProviderMock
            .Setup(x => x.GenerateTokenAsync(
                It.IsAny<User>(),
                It.IsAny<List<string>>(),
                It.IsAny<List<Claim>>()))
            .ReturnsAsync(loginResponse.AccessToken);

        // Act
        var result = await _identityService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(loginResponse.AccessToken, result);

        _userManagerMock.Verify(x => x.FindByNameAsync(loginRequest.Username), Times.Once);
        _userManagerMock.Verify(x => x.GetRolesAsync(user), Times.Once);
        _userManagerMock.Verify(x => x.GetClaimsAsync(user), Times.Once);
        _jwtProviderMock.Verify(x => x.GenerateTokenAsync(user, It.IsAny<List<string>>(), It.IsAny<List<Claim>>()), Times.Once);
    }
}