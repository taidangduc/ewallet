using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWallet.Wallet.DbConfigurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Entities.Transaction>
{
    public void Configure(EntityTypeBuilder<Entities.Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Type).HasConversion<string>();
        builder.Property(x => x.Status).HasConversion<string>();
    }
}