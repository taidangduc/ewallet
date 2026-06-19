using System.Security.Claims;
using EWallet.Common.Exceptions;
using EWallet.Contracts;
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
    private readonly Mock<IWalletClient> _walletClient;

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
        _walletClient = new Mock<IWalletClient>();

        _identityService = new IdentityService(_userManagerMock.Object, _signInManagerMock.Object, _jwtProviderMock.Object, _walletClient.Object);
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

    [Fact]
    public async Task CreateUserAsync_WhenUsernameAlreadyExists_ShouldThrowValidationException()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "user",
            Email = "user@example.com"
        };

        var request = new RegisterRequest
        {
            Username = "user",
            Email = "user@example.com",
            Password = "password"
        };

        _userManagerMock
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _identityService.CreateUserAsync(request.Username, request.Email, request.Password));
        _userManagerMock.Verify(x => x.FindByNameAsync(request.Username), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_WhenEmailAlreadyExists_ShouldThrowValidationException()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "peter",
            Email = "peter@example.com"
        };

        var request = new RegisterRequest
        {
            Username = "peterpan",
            Email = "user@example.com",
            Password = "password"
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _identityService.CreateUserAsync(request.Username, request.Email, request.Password));
        _userManagerMock.Verify(x => x.FindByEmailAsync(request.Email), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_WhenUserIsValid_ShouldCreateUser()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "user",
            Email = "user@example.com",
            Password = "password"
        };

        _userManagerMock
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddClaimsAsync(It.IsAny<User>(), It.IsAny<IEnumerable<Claim>>())).ReturnsAsync(IdentityResult.Success);

        _walletClient.Setup(x => x.CreateWalletAsync(It.IsAny<CreateWalletRequest>())).Returns(Task.CompletedTask);

        // Act
        await _identityService.CreateUserAsync(request.Username, request.Email, request.Password);

        // Assert
        _userManagerMock.Verify(x => x.FindByNameAsync(request.Username), Times.Once);
        _userManagerMock.Verify(x => x.FindByEmailAsync(request.Email), Times.Once);
        _userManagerMock.Verify(x => x.CreateAsync(It.Is<User>(u => u.UserName == request.Username && u.Email == request.Email), request.Password), Times.Once);
        _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        _userManagerMock.Verify(x => x.AddClaimsAsync(It.IsAny<User>(), It.IsAny<IEnumerable<Claim>>()), Times.Once);
        _walletClient.Verify(x => x.CreateWalletAsync(It.Is<CreateWalletRequest>(r => r.UserId == It.IsAny<Guid>())), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_WhenCreateWalletFails_ShouldThrowException()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "user",
            Email = "user@example.com",
            Password = "password"
        };

        _userManagerMock
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddClaimsAsync(It.IsAny<User>(), It.IsAny<IEnumerable<Claim>>())).ReturnsAsync(IdentityResult.Success);

        _walletClient.Setup(x => x.CreateWalletAsync(It.IsAny<CreateWalletRequest>())).Returns(Task.FromException(new Exception("Wallet service is down")));

        _userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _identityService.CreateUserAsync(request.Username, request.Email, request.Password));
        _userManagerMock.Verify(x => x.CreateAsync(It.Is<User>(u => u.UserName == request.Username && u.Email == request.Email), request.Password), Times.Once);
        _walletClient.Verify(x => x.CreateWalletAsync(It.Is<CreateWalletRequest>(r => r.UserId == It.IsAny<Guid>())), Times.Once);
        _userManagerMock.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Once);
    }
}