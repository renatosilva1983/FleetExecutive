using FleetExecutive.Domain.Veiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class GarageConfiguration : IEntityTypeConfiguration<Garage>
{
    public void Configure(EntityTypeBuilder<Garage> builder)
    {
        builder.ToTable("garages");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Nome).HasMaxLength(150).IsRequired();
        builder.Property(g => g.Cidade).HasMaxLength(150).IsRequired();
        builder.Property(g => g.Uf).HasMaxLength(2).IsRequired();
    }
}
