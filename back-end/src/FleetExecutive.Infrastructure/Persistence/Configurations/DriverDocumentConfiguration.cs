using FleetExecutive.Domain.Prestadores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class DriverDocumentConfiguration : IEntityTypeConfiguration<DriverDocument>
{
    public void Configure(EntityTypeBuilder<DriverDocument> builder)
    {
        builder.ToTable("driver_documents");
        builder.HasKey(d => d.Id);
        // Tipo: enum virou FK para tabela-catálogo (ver EnumLookupRegistry).
        builder.Property(d => d.Categoria).HasMaxLength(10);
        builder.Property(d => d.Numero).HasMaxLength(60);
        // O Status de validade não é coluna nem propriedade do agregado: vive na view
        // vw_driver_documents (fonte única) e é lido via DriverDocumentView. Ver DatabaseViews.
    }
}
