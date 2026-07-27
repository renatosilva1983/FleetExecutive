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
        builder.Property(q => q.Origem).HasConversion<string>().HasMaxLength(20);
        builder.Property(q => q.Status).HasConversion<string>().HasMaxLength(30);
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
