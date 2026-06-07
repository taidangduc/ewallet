using EWallet.Common.Core;
using EWallet.Common.Exceptions;
using EWallet.Contracts;
using EWallet.Wallet.Repositories;
using EWallet.Wallet.Services;
using Microsoft.AspNetCore.Http.Features;
using MockQueryable.Moq;
using Moq;

namespace EWallet.Wallet.UnitTests.Services;

public class WalletServiceTests
{
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly WalletService _walletService;

    public WalletServiceTests()
    {
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _walletRepositoryMock
            .Setup(x => x.GetQueryable())
            .Returns(new List<Entities.Wallet>().AsQueryable());

        _walletRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Entities.Wallet>(), default))
            .Returns(Task.CompletedTask);

        _walletRepositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(_unitOfWorkMock.Object);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(default))
            .ReturnsAsync(1);

        _walletService = new WalletService(_walletRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateWalletAsync_ShouldCreateWallet()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var wallet = new List<Entities.Wallet>
        {
            new Entities.Wallet
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Balance = 0,
                Currency = "USD",
                CreatedDateTime = DateTimeOffset.UtcNow
            }
        };

        var model = new CreateWalletRequest
        {
            UserId = userId
        };

        _walletRepositoryMock
            .Setup(x => x.GetQueryable())
            .Returns(wallet.AsQueryable().BuildMockDbSet().Object);

        // Act
        await _walletService.CreateWalletAsync(model);

        // Assert
        _walletRepositoryMock.Verify(x =>
            x.AddAsync(
                It.Is<Entities.Wallet>(y => y.UserId == model.UserId && y.Balance == 0 && y.Currency == "USD"),
                default),
            Times.Once);

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task CreateWalletAsync_WhenWalletAlreadyExists_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var wallet = new List<Entities.Wallet>
        {
            new Entities.Wallet
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Balance = 0,
                Currency = "USD",
                CreatedDateTime = DateTimeOffset.UtcNow
            }
        };

        var model = new CreateWalletRequest
        {
            UserId = userId
        };

        _walletRepositoryMock
            .Setup(x => x.GetQueryable())
            .Returns(wallet.AsQueryable().BuildMockDbSet().Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _walletService.CreateWalletAsync(model));
        _walletRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Entities.Wallet>(), default), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}