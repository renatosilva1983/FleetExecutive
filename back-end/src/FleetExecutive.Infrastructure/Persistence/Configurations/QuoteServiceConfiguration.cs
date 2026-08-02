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
        // TipoServico / Subtipo: enums viraram FK para tabela-catálogo (ver EnumLookupRegistry).
        builder.Property(s => s.Origem).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Destino).HasMaxLength(200).IsRequired();
        // TipoVeiculoPreferido (nullable): enum virou FK opcional para tabela-catálogo (ver EnumLookupRegistry).

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
