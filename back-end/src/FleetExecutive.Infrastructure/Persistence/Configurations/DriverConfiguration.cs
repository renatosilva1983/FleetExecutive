using System.Text.Json;
using FleetExecutive.Domain.Prestadores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("drivers");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Nome).HasMaxLength(200).IsRequired();
        // Tipo: enum virou FK para tabela-catálogo (ver EnumLookupRegistry).
        builder.Property(d => d.ComissaoPercentual).HasColumnType("numeric(5,2)");
        builder.HasIndex(d => d.Nome);
        builder.HasIndex(d => d.Referencia);

        // Capacidades: mesma técnica de Customer.Tags / Vehicle.Features — JSON via campo privado.
        builder.Property<List<DriverCapability>>("_capacidades")
            .HasColumnName("capacidades")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<DriverCapability>>(v, (JsonSerializerOptions?)null) ?? new List<DriverCapability>())
            .Metadata.SetValueComparer(new ValueComparer<List<DriverCapability>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
        builder.Ignore(d => d.Capacidades);

        builder.HasMany(d => d.Idiomas)
            .WithOne()
            .HasForeignKey(l => l.DriverId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Metadata.FindNavigation(nameof(Driver.Idiomas))!.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(d => d.Documentos)
            .WithOne()
            .HasForeignKey(doc => doc.DriverId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Metadata.FindNavigation(nameof(Driver.Documentos))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
