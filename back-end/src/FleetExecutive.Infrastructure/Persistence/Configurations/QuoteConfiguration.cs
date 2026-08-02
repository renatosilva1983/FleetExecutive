using FleetExecutive.Domain.Orcamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        builder.ToTable("quotes");
        builder.HasKey(q => q.Id);
        // Origem / Status: enums viraram FK para tabela-catálogo (ver EnumLookupRegistry).
        builder.Property(q => q.ValorEstimado).HasColumnType("numeric(12,2)");
        builder.HasIndex(q => q.Status);
        builder.HasIndex(q => q.CustomerId);

        builder.HasMany(q => q.Servicos)
            .WithOne()
            .HasForeignKey(s => s.QuoteId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Metadata.FindNavigation(nameof(Quote.Servicos))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
