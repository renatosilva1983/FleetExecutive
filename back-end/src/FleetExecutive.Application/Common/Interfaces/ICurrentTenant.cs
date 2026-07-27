namespace FleetExecutive.Application.Common.Interfaces;

/// <summary>
/// Única fonte de verdade sobre o tenant corrente dentro de um handler da Application layer.
/// Nunca resolver tenant a partir de um campo do DTO da requisição — ver
/// Estrutura/07-autenticacao-seguranca-rbac.md, seção "Anti-tampering".
/// </summary>
public interface ICurrentTenant
{
    Guid? TenantId { get; }
    string? Identificador { get; }
    bool IsResolved { get; }
}
