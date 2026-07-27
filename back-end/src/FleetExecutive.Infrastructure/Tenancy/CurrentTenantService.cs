using FleetExecutive.Application.Common.Interfaces;
using Finbuckle.MultiTenant.Abstractions;

namespace FleetExecutive.Infrastructure.Tenancy;

/// <summary>
/// Implementação de ICurrentTenant a partir do tenant resolvido pelo Finbuckle.MultiTenant
/// (resolução por Host, ver DependencyInjection.cs). Fonte única — nunca ler tenant de outro
/// lugar (ver Estrutura/07-autenticacao-seguranca-rbac.md, "Anti-tampering").
/// </summary>
public class CurrentTenantService : ICurrentTenant
{
    private readonly IMultiTenantContextAccessor _accessor;

    public CurrentTenantService(IMultiTenantContextAccessor accessor)
    {
        _accessor = accessor;
    }

    private ITenantInfo? TenantInfo => _accessor.MultiTenantContext?.TenantInfo;

    public Guid? TenantId => Guid.TryParse(TenantInfo?.Id, out var id) ? id : null;
    public string? Identificador => TenantInfo?.Identifier;
    public bool IsResolved => TenantInfo is not null;
}
