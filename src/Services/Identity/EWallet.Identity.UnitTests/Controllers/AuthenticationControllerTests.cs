using EWallet.Identity.Controllers;
using EWallet.Identity.Entities;
using EWallet.Identity.Models;
using EWallet.Identity.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EWallet.Identity.UnitTests.Controllers;

//ref: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/test-min-api?view=aspnetcore-10.0
//ref: https://samueleresca.net/unit-testing-asp-net-core-identity/
public class AuthenticationControllerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly AuthenticationController _controller;

    public AuthenticationControllerTests()
    {
        _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null);

        _signInManagerMock = new Mock<SignInManager<User>>(
            _userManagerMock.Object,
            new Mock<HttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            null, null, null, null);

        _identityServiceMock = new Mock<IIdentityService>();

        _controller = new AuthenticationController(_userManagerMock.Object, _signInManagerMock.Object, _identityServiceMock.Object);
    }

    [Fact]

    public async Task Login_WhenUserValid_ShouldReturnToken()
    {
        // Arrange
        var loginModel = new LoginModel
        {
            Username = "peter",
            Password = "password123"
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = loginModel.Username
        };

        var authModel = new AuthenticationModel
        {
            AccessToken = "fake-jwt-token"
        };

        _userManagerMock
            .Setup(x => x.FindByNameAsync(loginModel.Username))
            .ReturnsAsync(user);

        _signInManagerMock
            .Setup(x => x.PasswordSignInAsync(user, loginModel.Password, false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        _identityServiceMock
            .Setup(x => x.AuthenticateAsync(loginModel))
            .ReturnsAsync(authModel);

        // Act
        var response = await _controller.Login(loginModel);

        // Assert
        var result = Assert.IsType<OkObjectResult>(response);
        var model = Assert.IsType<AuthenticationModel>(result.Value);

        Assert.Equal(authModel.AccessToken, model.AccessToken);
    }
}