using Microsoft.EntityFrameworkCore;

namespace EWallet.Wallet.Data;

public class WalletDbContextSeed
{
    private readonly WalletDbContext _context;
    private readonly IEnumerable<Entities.Wallet> _wallets = new List<Entities.Wallet>
    {
        new Entities.Wallet
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000001"),
            UserId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            Balance = 1000000,
            Currency = "USD",
            CreatedDateTime = DateTimeOffset.UtcNow,
        },
        new Entities.Wallet
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000002"),
            UserId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
            Balance = 500000,
            Currency = "USD",
            CreatedDateTime = DateTimeOffset.UtcNow,
        }
    };

    public WalletDbContextSeed(WalletDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await SeedWalletsAsync();
    }

    public async Task SeedWalletsAsync()
    {
        if (!await _context.Wallets.AnyAsync())
        {
            await _context.Wallets.AddRangeAsync(_wallets);
            await _context.SaveChangesAsync();
        }
    }
}