using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWallet.Wallet.DbConfigurations;

public class WalletConfiguration : IEntityTypeConfiguration<Entities.Wallet>
{
    public void Configure(EntityTypeBuilder<Entities.Wallet> builder)
    {
        builder.ToTable("Wallets");
        builder.Property(x => x.Balance).HasColumnType("decimal(18,2)");
        builder.HasMany(x => x.Transactions)
            .WithOne()
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}