using System.Text.Json;
using FleetExecutive.Domain.Prestadores;
using FleetExecutive.Domain.Veiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.NumeroOrdem).HasMaxLength(20).IsRequired();
        builder.Property(v => v.Placa).HasMaxLength(10).IsRequired();
        builder.HasIndex(v => v.Placa).IsUnique();
        // Status: enum virou FK para tabela-catálogo (ver EnumLookupRegistry).

        // Features: mesma técnica de Customer.Tags — coleção guardada como JSON via campo privado.
        builder.Property<List<VehicleFeature>>("_features")
            .HasColumnName("features")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<VehicleFeature>>(v, (JsonSerializerOptions?)null) ?? new List<VehicleFeature>())
            .Metadata.SetValueComparer(new ValueComparer<List<VehicleFeature>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
        builder.Ignore(v => v.Features);

        builder.HasOne<Fleet>().WithMany().HasForeignKey(v => v.FleetId).OnDelete(DeleteBehavior.Restrict);

        // Resolve a pendência registrada em docs/adr/0004-modulo-veiculos-frotas.md: agora que
        // Driver existe, o vínculo de motorista habitual vira uma FK real (SetNull ao excluir o
        // prestador, nunca bloquear a exclusão por causa de um vínculo "habitual" opcional).
        builder.HasOne<Driver>().WithMany().HasForeignKey(v => v.MotoristaHabitualId).OnDelete(DeleteBehavior.SetNull);
    }
}
