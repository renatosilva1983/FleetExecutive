using FleetExecutive.Domain.Prestadores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class DriverLanguageConfiguration : IEntityTypeConfiguration<DriverLanguage>
{
    public void Configure(EntityTypeBuilder<DriverLanguage> builder)
    {
        builder.ToTable("driver_languages");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Idioma).HasMaxLength(60).IsRequired();
        // Nivel: enum virou FK para tabela-catálogo (ver EnumLookupRegistry).
    }
}
