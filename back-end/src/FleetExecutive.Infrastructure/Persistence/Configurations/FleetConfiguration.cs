using FleetExecutive.Domain.Veiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class FleetConfiguration : IEntityTypeConfiguration<Fleet>
{
    public void Configure(EntityTypeBuilder<Fleet> builder)
    {
        builder.ToTable("fleets");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Titulo).HasMaxLength(200).IsRequired();
        // Tipo / Categoria: enums viraram FK para tabela-catálogo (ver EnumLookupRegistry).
    }
}
