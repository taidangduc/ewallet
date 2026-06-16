using EWallet.Common.Web;
using EWallet.Identity.Models;
using EWallet.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly ICurrentWebUser _currentWebUser;

    public AuthenticationController(IIdentityService identityService, ICurrentWebUser currentWebUser)
    {
        _identityService = identityService;
        _currentWebUser = currentWebUser;
    }

    [HttpGet, Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get()
    {
        var userId = _currentWebUser.UserId;

        if (Guid.Empty.Equals(userId) || userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var response = await _identityService.GetUserAsync(userId);

        return Ok(response);
    }

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var authenticate = await _identityService.AuthenticateAsync(request.Username, request.Password);

        var response = new LoginResponse
        {
            AccessToken = authenticate
        };

        return Ok(response);
    }
}