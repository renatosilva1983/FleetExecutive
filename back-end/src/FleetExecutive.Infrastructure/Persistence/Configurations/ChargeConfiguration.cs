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
        // Tipo / Status / Origem: enums viraram FK para tabela-catálogo (ver EnumLookupRegistry).
        builder.Property(c => c.Valor).HasColumnType("numeric(12,2)");
        builder.Property(c => c.ImpostoRetido).HasColumnType("numeric(12,2)");
        builder.HasIndex(c => c.OrderId);
        builder.HasIndex(c => c.Status);
    }
}
