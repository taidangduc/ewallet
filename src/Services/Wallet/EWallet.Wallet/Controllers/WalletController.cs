using EWallet.Common.Web;
using EWallet.Contracts;
using EWallet.Wallet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Wallet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly WalletService _walletService;
    private readonly ICurrentWebUser _currentWebUser;

    public WalletController(WalletService walletService, ICurrentWebUser currentWebUser)
    {
        _walletService = walletService;
        _currentWebUser = currentWebUser;
    }

    [HttpGet, Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get()
    {
        var userId = _currentWebUser.UserId;

        if (Guid.Empty.Equals(userId) || userId == null)
        {
            return Unauthorized();
        }

        var wallet = await _walletService.GetWalletAsync(userId);

        return Ok(wallet);
    }

    [HttpPost, Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateWalletRequest request)
    {
        await _walletService.CreateWalletAsync(request);

        return Ok();
    }
}