using FleetExecutive.Application.Prestadores.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetExecutive.Infrastructure.Persistence.Configurations;

/// <summary>
/// Mapeia o read model à view vw_driver_documents (somente leitura, sem chave). A view é criada/
/// atualizada na publicação pelo MigrationRunner (ver DatabaseViews) — não é gerenciada por
/// migration, por isso não aparece no snapshot como tabela.
/// </summary>
public class DriverDocumentViewConfiguration : IEntityTypeConfiguration<DriverDocumentView>
{
    public void Configure(EntityTypeBuilder<DriverDocumentView> builder)
    {
        builder.HasNoKey();
        builder.ToView("vw_driver_documents");
    }
}
