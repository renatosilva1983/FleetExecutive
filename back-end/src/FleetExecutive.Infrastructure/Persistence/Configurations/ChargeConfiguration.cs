using FleetExecutive.Domain.Financeiro;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class ChargeConfiguration : IEntityTypeConfiguration<Charge>
{
    public void Configure(EntityTypeBuilder<Charge> builder)
    {
        builder.ToTable("charges");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Tipo).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.Origem).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.Valor).HasColumnType("numeric(12,2)");
        builder.Property(c => c.ImpostoRetido).HasColumnType("numeric(12,2)");
        builder.HasIndex(c => c.OrderId);
        builder.HasIndex(c => c.Status);
    }
}
