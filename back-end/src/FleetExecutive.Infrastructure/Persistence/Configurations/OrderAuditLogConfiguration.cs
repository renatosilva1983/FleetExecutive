using FleetExecutive.Domain.Pedidos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class OrderAuditLogConfiguration : IEntityTypeConfiguration<OrderAuditLog>
{
    public void Configure(EntityTypeBuilder<OrderAuditLog> builder)
    {
        builder.ToTable("order_audit_logs");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.TipoEvento).HasMaxLength(30).IsRequired();
        builder.Property(l => l.Descricao).IsRequired();
        builder.HasIndex(l => l.OrderId);
    }
}
