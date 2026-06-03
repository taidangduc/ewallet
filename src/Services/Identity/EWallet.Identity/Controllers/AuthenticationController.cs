using EWallet.Common.Exceptions;
using EWallet.Common.Web;
using EWallet.Identity.DTOs;
using EWallet.Identity.Entities;
using EWallet.Identity.Models;
using EWallet.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IIdentityService _identityService;
    private readonly ICurrentWebUser _currentWebUser;

    public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, IIdentityService identityService, ICurrentWebUser currentWebUser)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _identityService = identityService;
        _currentWebUser = currentWebUser;
    }

    [HttpGet, Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Get()
    {
        var userId = _currentWebUser.UserId;

        if (Guid.Empty.Equals(userId))
        {
            return Unauthorized();
        }

        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return Unauthorized();
        }

        var roles = _userManager.GetRolesAsync(user).Result.ToList();

        var userDto = new UserDTO
        {
            Id = userId,
            Username = user.UserName,
            Roles = roles
        };

        return Ok(userDto);
    }

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginModel request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
        if (!result.Succeeded)
        {
            return BadRequest("Invalid username or password");
        }
        var authenticationModel = await _identityService.AuthenticateAsync(request);

        return Ok(authenticationModel);
    }
}