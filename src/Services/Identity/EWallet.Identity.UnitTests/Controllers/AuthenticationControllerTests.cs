using EWallet.Common.Web;
using EWallet.Identity.Controllers;
using EWallet.Identity.Models;
using EWallet.Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EWallet.Identity.UnitTests.Controllers;

//ref: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/test-min-api?view=aspnetcore-10.0
//ref: https://samueleresca.net/unit-testing-asp-net-core-identity/
public class AuthenticationControllerTests
{
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly AuthenticationController _controller;
    private readonly Mock<ICurrentWebUser> _currentWebUserMock;

    public AuthenticationControllerTests()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _currentWebUserMock = new Mock<ICurrentWebUser>();

        _controller = new AuthenticationController(_identityServiceMock.Object, _currentWebUserMock.Object);
    }

    [Fact]
    public async Task Login_WhenUserValid_ShouldReturnToken()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Username = "peter",
            Password = "password123"
        };

        string fakeToken = "fake-jwt-token";

        var loginResponse = new LoginResponse
        {
            AccessToken = fakeToken
        };

        _identityServiceMock
            .Setup(x => x.AuthenticateAsync(loginRequest.Username, loginRequest.Password))
            .ReturnsAsync(fakeToken);

        // Act
        var response = await _controller.Login(loginRequest);

        // Assert
        var result = Assert.IsType<OkObjectResult>(response);
        var model = Assert.IsType<LoginResponse>(result.Value);

        Assert.Equal(fakeToken, model.AccessToken);
    }
}