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
        builder.Property(d => d.Tipo).HasConversion<string>().HasMaxLength(20);
        builder.Property(d => d.Categoria).HasMaxLength(10);
        builder.Property(d => d.Numero).HasMaxLength(60);
        // "Status" é calculado em runtime a partir de ValidoAte (ver DriverDocument.Status) — não é coluna.
        builder.Ignore(d => d.Status);
    }
}
