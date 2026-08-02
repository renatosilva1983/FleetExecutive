using FleetExecutive.Domain.Pedidos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");
        builder.HasKey(i => i.Id);
        // Tipo (SubtipoServico): enum virou FK para tabela-catálogo (ver EnumLookupRegistry).
        builder.Property(i => i.Origem).HasMaxLength(200).IsRequired();
        builder.Property(i => i.Destino).HasMaxLength(200).IsRequired();
        builder.Property(i => i.ValorServico).HasColumnType("numeric(12,2)");
        builder.Property(i => i.Acrescimo).HasColumnType("numeric(12,2)");
        builder.Ignore(i => i.Subtotal);
        builder.Property(i => i.ChaveAcessoCheckin).HasMaxLength(32).IsRequired();
        builder.HasIndex(i => i.ChaveAcessoCheckin).IsUnique();
    }
}
