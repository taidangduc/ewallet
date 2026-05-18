using EWallet.Wallet.Models;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Wallet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> Get()
    {
        return Task.FromResult<IActionResult>(Ok(new { Transactions = new string[] { "Transaction1", "Transaction2" } }));
    }

    // NOTE:
    // You can separate 2 endpoints "Deposit" "+" and "Withdraw" "-"
    // Here, with demo purpose, I just use one endpoint for both operations
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<IActionResult> Update([FromBody] TransactionModel request)
    {
        if (request.Amount <= 0)
        {
            return Task.FromResult<IActionResult>(BadRequest("Amount must be greater than zero."));
        }

        return Task.FromResult<IActionResult>(Ok(new { Message = "Transaction processed successfully." }));
    }
}