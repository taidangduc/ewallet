using EWallet.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Wallet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> Get()
    {
        Console.WriteLine("Getting wallet balance");
        return Task.FromResult<IActionResult>(Ok(new { Balance = 1000 }));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateWalletModel model)
    {
        Console.WriteLine($"Creating wallet for user {model.UserId}");
        await _walletService.CreateWalletAsync(model);
        return Ok();
    }
}