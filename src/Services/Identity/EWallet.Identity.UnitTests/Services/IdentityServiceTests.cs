using System.Security.Claims;
using EWallet.Common.Exceptions;
using EWallet.Identity.Entities;
using EWallet.Identity.Models;
using EWallet.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace EWallet.Identity.UnitTests.Services;

public class IdentityServiceTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly IdentityService _identityService;

    public IdentityServiceTests()
    {
        _userManagerMock = new Mock<UserManager<User>>(
           new Mock<IUserStore<User>>().Object,
           null, null, null, null, null, null, null, null);
        _jwtProviderMock = new Mock<IJwtProvider>();
        _identityService = new IdentityService(_userManagerMock.Object, _jwtProviderMock.Object);
    }

    [Fact]
    public async Task AuthenticateAsync_WhenUserNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var loginModel = new LoginModel
        {
            Username = "unknown",
            Password = "password"
        };

        _userManagerMock
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _identityService.AuthenticateAsync(loginModel));
        _userManagerMock.Verify(x => x.FindByNameAsync(loginModel.Username), Times.Once);
        _userManagerMock.Verify(x => x.GetRolesAsync(It.IsAny<User>()), Times.Never);
        _userManagerMock.Verify(x => x.GetClaimsAsync(It.IsAny<User>()), Times.Never);
        _jwtProviderMock.Verify(x => x.GenerateTokenAsync(It.IsAny<User>(), It.IsAny<List<string>>(), It.IsAny<List<Claim>>()), Times.Never);
    }

    [Fact]
    public async Task AuthenticateAsync_WhenUserIsValid_ShouldReturnAuthenticationModel()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "user",
        };

        var loginModel = new LoginModel
        {
            Username = "user",
            Password = "password"
        };

        var authModel = new AuthenticationModel
        {
            AccessToken = "fake-jwt-token"
        };

        _userManagerMock
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

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
            .ReturnsAsync(authModel.AccessToken);

        // Act
        var result = await _identityService.AuthenticateAsync(loginModel);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<AuthenticationModel>(result);
        Assert.Equal(authModel.AccessToken, result.AccessToken);

        _userManagerMock.Verify(x => x.FindByNameAsync(loginModel.Username), Times.Once);
        _userManagerMock.Verify(x => x.GetRolesAsync(user), Times.Once);
        _userManagerMock.Verify(x => x.GetClaimsAsync(user), Times.Once);
        _jwtProviderMock.Verify(x => x.GenerateTokenAsync(user, It.IsAny<List<string>>(), It.IsAny<List<Claim>>()), Times.Once);
    }
}