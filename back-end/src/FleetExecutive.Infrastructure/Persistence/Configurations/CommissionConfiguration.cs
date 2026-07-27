using FleetExecutive.Domain.Financeiro;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class CommissionConfiguration : IEntityTypeConfiguration<Commission>
{
    public void Configure(EntityTypeBuilder<Commission> builder)
    {
        builder.ToTable("commissions");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Tipo).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.RecebedorTipo).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.Percentual).HasColumnType("numeric(5,2)");
        builder.Property(c => c.Valor).HasColumnType("numeric(12,2)");
        builder.HasIndex(c => c.OrderItemId);
        builder.HasIndex(c => new { c.RecebedorTipo, c.RecebedorId });
    }
}
