using EWallet.Common.Web;
using EWallet.Wallet.Models;
using EWallet.Wallet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Wallet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ICurrentWebUser _currentWebUser;
    private readonly TransactionService _transactionService;
    public TransactionController(ICurrentWebUser currentWebUser, TransactionService transactionService)
    {
        _currentWebUser = currentWebUser;
        _transactionService = transactionService;
    }

    [HttpGet, Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get()
    {
        var userId = _currentWebUser.UserId;
        var transactions = await _transactionService.GetTransactionsAsync(userId);
        return Ok(transactions);
    }

    // NOTE:
    // You can separate 2 endpoints "Deposit" "+" and "Withdraw" "-"
    // Here, with demo purpose, I just use one endpoint for both operations
    [HttpPost, Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] TransactionModel request)
    {
        if (request.Amount <= 0)
        {
            return BadRequest("Amount must be greater than zero.");
        }

        await _transactionService.CreateTransactionAsync(request);
        return Ok(new { Message = "Transaction processed successfully." });
    }
}