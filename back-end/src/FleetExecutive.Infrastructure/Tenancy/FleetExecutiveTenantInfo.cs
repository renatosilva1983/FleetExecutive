using Finbuckle.MultiTenant.Abstractions;

namespace FleetExecutive.Infrastructure.Tenancy;

/// <summary>
/// Implementação de ITenantInfo usada pelo Finbuckle.MultiTenant para resolver o tenant a partir
/// do Host da requisição (ver Estrutura/ESTRUTURA-PROJETO.md seção 4.3 e
/// Estrutura/07-autenticacao-seguranca-rbac.md). "Identifier" é o domínio (Host) usado na
/// resolução; "ConnectionString" é a connection string do banco daquele tenant.
/// </summary>
public class FleetExecutiveTenantInfo : ITenantInfo
{
    public string? Id { get; set; }
    public string? Identifier { get; set; }
    public string? Name { get; set; }
    public string? ConnectionString { get; set; }
    public string? LogoUrl { get; set; }
    public string? CorPrimaria { get; set; }
}
