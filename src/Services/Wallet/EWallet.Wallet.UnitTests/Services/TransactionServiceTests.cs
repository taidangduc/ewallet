using EWallet.Common.Core;
using EWallet.Common.Exceptions;
using EWallet.Wallet.ExternalServices.Payment;
using EWallet.Wallet.Models;
using EWallet.Wallet.Repositories;
using EWallet.Wallet.Services;
using MockQueryable.Moq;
using Moq;

namespace EWallet.Wallet.UnitTests.Services;

// ref: https://www.nuget.org/packages/MockQueryable.Moq
public class TransactionServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly Mock<IPaymentGateway> _paymentGatewayMock;
    private readonly TransactionService _transactionService;

    public TransactionServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _paymentGatewayMock = new Mock<IPaymentGateway>();

        _unitOfWorkMock
            .Setup(x => x.BeginTransactionAsync(default))
            .ReturnsAsync(new Mock<IDisposable>().Object);

        _unitOfWorkMock
            .Setup(x => x.CommitTransactionAsync(default))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(default))
            .ReturnsAsync(1);

        _transactionRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Entities.Transaction>(), default))
            .Returns(Task.CompletedTask);

        _transactionService = new TransactionService(
            _transactionRepositoryMock.Object,
            _walletRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _paymentGatewayMock.Object);
    }

    [Fact]
    public async Task GetTransactionsAsync_ShouldReturnTransactions()
    {
        // Arrange
        var walletId = Guid.NewGuid();
        var transactions = new List<Entities.Transaction>
        {
            new Entities.Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = walletId,
                Type = Entities.TransactionType.Deposit,
                Amount = 50,
                Description = "Deposit Transaction",
                CreatedDateTime = DateTime.UtcNow
            },
            new Entities.Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = walletId,
                Type = Entities.TransactionType.Withdraw,
                Amount = 100,
                Description = "Withdraw Transaction",
                CreatedDateTime = DateTime.UtcNow
            }
        };

        _transactionRepositoryMock
            .Setup(x => x.GetQueryable())
            .Returns(transactions.BuildMockDbSet().Object);

        // Act
        var result = await _transactionService.GetTransactionsAsync(walletId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(transactions[0].Id, result[0].Id);
        Assert.Equal(transactions[1].Id, result[1].Id);
    }

    [Fact]
    public async Task CreateTransactionAsync_WhenDepositSucceed_ShouldCreateTransaction()
    {
        // Arrange
        var walletId = Guid.NewGuid();
        var model = new TransactionModel
        {
            WalletId = walletId,
            Type = Entities.TransactionType.Deposit,
            Amount = 100,
            CardNumber = "424242424242424242"
        };

        var wallet = new Entities.Wallet
        {
            Id = walletId,
            Balance = 0
        };

        _walletRepositoryMock
            .Setup(x => x.GetQueryable())
            .Returns(new List<Entities.Wallet> { wallet }.BuildMockDbSet().Object);

        _paymentGatewayMock
            .Setup(x => x.ChargeAsync(It.IsAny<PaymentRequest>()))
            .ReturnsAsync(new PaymentResponse { Success = true });

        // Act
        await _transactionService.CreateTransactionAsync(model);

        // Assert
        Assert.Equal(100, wallet.Balance);
        _paymentGatewayMock.Verify(x => x.ChargeAsync(It.IsAny<PaymentRequest>()), Times.Once);
        _transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Entities.Transaction>(), default), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(default), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task CreateTransactionAsync_WhenChargeFailed_ShouldThrowException()
    {
        // Arrange
        var walletId = Guid.NewGuid();
        var model = new TransactionModel
        {
            WalletId = walletId,
            Type = Entities.TransactionType.Deposit,
            Amount = 100,
            CardNumber = "400000000000000002"
        };

        var wallet = new Entities.Wallet
        {
            Id = walletId,
            Balance = 0
        };

        _walletRepositoryMock
            .Setup(x => x.GetQueryable())
            .Returns(new List<Entities.Wallet> { wallet }.BuildMockDbSet().Object);

        _paymentGatewayMock
            .Setup(x => x.ChargeAsync(It.IsAny<PaymentRequest>()))
            .ReturnsAsync(new PaymentResponse { Success = false, Message = "Card declined" });

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _transactionService.CreateTransactionAsync(model));
        Assert.Equal(0, wallet.Balance);
        _paymentGatewayMock.Verify(x => x.ChargeAsync(It.IsAny<PaymentRequest>()), Times.Once);
        _transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Entities.Transaction>(), default), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(default), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task CreateTransactionAsync_WhenInsufficientBalance_ShouldThrowException()
    {
        // Arrange
        var walletId = Guid.NewGuid();
        var model = new TransactionModel
        {
            WalletId = walletId,
            Type = Entities.TransactionType.Withdraw,
            Amount = 100
        };

        var wallet = new Entities.Wallet
        {
            Id = walletId,
            Balance = 50
        };

        _walletRepositoryMock
            .Setup(x => x.GetQueryable())
            .Returns(new List<Entities.Wallet> { wallet }.BuildMockDbSet().Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _transactionService.CreateTransactionAsync(model));
        Assert.Equal(50, wallet.Balance);
        _paymentGatewayMock.Verify(x => x.PayoutAsync(It.IsAny<PayoutRequest>()), Times.Never);
        _transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Entities.Transaction>(), default), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(default), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}