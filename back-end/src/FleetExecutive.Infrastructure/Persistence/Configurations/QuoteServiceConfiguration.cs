using System.Text.Json;
using FleetExecutive.Domain.Orcamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class QuoteServiceConfiguration : IEntityTypeConfiguration<QuoteService>
{
    public void Configure(EntityTypeBuilder<QuoteService> builder)
    {
        builder.ToTable("quote_services");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.TipoServico).HasConversion<string>().HasMaxLength(30);
        builder.Property(s => s.Subtipo).HasConversion<string>().HasMaxLength(30);
        builder.Property(s => s.Origem).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Destino).HasMaxLength(200).IsRequired();
        builder.Property(s => s.TipoVeiculoPreferido).HasConversion<string>().HasMaxLength(20);

        builder.Property<List<CaracteristicaServico>>("_caracteristicas")
            .HasColumnName("caracteristicas")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<CaracteristicaServico>>(v, (JsonSerializerOptions?)null) ?? new List<CaracteristicaServico>())
            .Metadata.SetValueComparer(new ValueComparer<List<CaracteristicaServico>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
        builder.Ignore(s => s.Caracteristicas);
    }
}
