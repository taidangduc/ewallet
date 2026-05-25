using EWallet.Common.Web;
using EWallet.Wallet.Controllers;
using EWallet.Wallet.Models;
using EWallet.Wallet.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EWallet.Wallet.UnitTests.Controllers;

public class TransactionControllerTests
{
    private readonly Mock<ICurrentWebUser> _currentWebUserMock;
    private readonly Mock<ITransactionService> _transactionServiceMock;
    private readonly TransactionController _controller;

    public TransactionControllerTests()
    {
        _currentWebUserMock = new Mock<ICurrentWebUser>();
        _transactionServiceMock = new Mock<ITransactionService>();

        _currentWebUserMock.Setup(x => x.UserId).Returns(Guid.NewGuid());
        _controller = new TransactionController(_currentWebUserMock.Object, _transactionServiceMock.Object);
    }

    [Fact]
    public async Task CreateTransaction_WhenAmountIsZero_ShouldThenReturnBadRequest()
    {
        // Arrange
        var request = new TransactionModel
        {
            WalletId = Guid.NewGuid(),
            Amount = 0,
            Type = Entities.TransactionType.Deposit,
            CardNumber = "4242424242424242"
        };

        // Act
        var result = await _controller.Create(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}