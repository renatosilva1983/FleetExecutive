using FleetExecutive.Domain.Tarefas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("tasks");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Titulo).HasMaxLength(300).IsRequired();
        // Prioridade / Status / VinculoTipo: enums viraram FK para tabela-catálogo (ver EnumLookupRegistry).
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.ResponsavelId);
        builder.HasIndex(t => new { t.VinculoTipo, t.VinculoId });
    }
}
