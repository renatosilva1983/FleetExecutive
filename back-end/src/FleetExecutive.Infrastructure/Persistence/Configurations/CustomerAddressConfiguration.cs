using FleetExecutive.Domain.Clientes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.ToTable("customer_addresses");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Tipo).HasConversion<string>().HasMaxLength(20);
        builder.Property(a => a.Rua).HasMaxLength(300).IsRequired();
        builder.Property(a => a.Cidade).HasMaxLength(150).IsRequired();
        builder.Property(a => a.Uf).HasMaxLength(2).IsRequired();
        builder.Property(a => a.Cep).HasMaxLength(10);
    }
}
