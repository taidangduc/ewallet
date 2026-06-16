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
    private readonly ITransactionService _transactionService;
    public TransactionController(ICurrentWebUser currentWebUser, ITransactionService transactionService)
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

        if (Guid.Empty.Equals(userId) || userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var response = await _transactionService.GetTransactionsAsync(userId);

        return Ok(response);
    }

    // NOTE:
    // You can separate 2 endpoints "Deposit" "+" and "Withdraw" "-"
    // Here, with demo purpose, I just use one endpoint for both operations
    [HttpPost, Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request)
    {
        if (request.Amount <= 0)
        {
            return BadRequest();
        }

        var model = new CreateTransactionModel
        {
            UserId = _currentWebUser.UserId,
            Amount = request.Amount,
            Type = request.Type,
            CardId = request.CardId
        };

        await _transactionService.CreateTransactionAsync(model);

        return Created();
    }
}