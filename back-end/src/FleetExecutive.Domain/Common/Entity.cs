namespace FleetExecutive.Domain.Common;

/// <summary>
/// Base para toda entidade de negócio dentro de um banco de tenant.
/// Ver Estrutura/06-modelo-de-dados.md — convenção de auditoria (created_by/updated_by)
/// é aplicada nas entidades que participam do padrão de auditoria transversal.
/// </summary>
public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public Guid? CreatedBy { get; protected set; }
    public Guid? UpdatedBy { get; protected set; }

    public void MarkUpdated(Guid? updatedBy)
    {
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;
    }
}
