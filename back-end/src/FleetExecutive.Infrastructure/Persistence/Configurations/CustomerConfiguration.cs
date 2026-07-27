using System.Text.Json;
using FleetExecutive.Domain.Clientes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nome).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(320).IsRequired();
        builder.Property(c => c.CpfCnpj).HasMaxLength(20);
        builder.Property(c => c.Tipo).HasConversion<string>().HasMaxLength(20);
        builder.HasIndex(c => c.Email);
        builder.HasIndex(c => c.CpfCnpj);
        builder.HasIndex(c => c.Nome);

        // Tags: coleção simples guardada como JSON em vez de tabela N:N — ver
        // Estrutura/06-modelo-de-dados.md ("tags (array ou tabela N:N customer_tags)").
        builder.Property<List<string>>("_tags")
            .HasColumnName("tags")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
        builder.Ignore(c => c.Tags);

        builder.HasMany(c => c.Enderecos)
            .WithOne()
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Metadata.FindNavigation(nameof(Customer.Enderecos))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
