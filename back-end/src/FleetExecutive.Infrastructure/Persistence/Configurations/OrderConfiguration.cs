using FleetExecutive.Domain.Pedidos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(o => o.Id);
        // Origem / StatusComercial / StatusOperacional: enums viraram FK para tabela-catálogo (ver EnumLookupRegistry).
        builder.Property(o => o.ValorTotal).HasColumnType("numeric(12,2)");
        builder.HasIndex(o => o.StatusComercial);
        builder.HasIndex(o => o.CustomerId);
        builder.HasIndex(o => o.QuoteId).IsUnique();

        builder.HasMany(o => o.Itens)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Metadata.FindNavigation(nameof(Order.Itens))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
