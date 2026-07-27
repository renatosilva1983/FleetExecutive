using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Microsoft.EntityFrameworkCore;
using FleetExecutive.Infrastructure.Tenancy;

namespace FleetExecutive.Infrastructure.Persistence;

/// <summary>
/// Banco "master" (Estrutura/06-modelo-de-dados.md) — só o cadastro de tenants usado na
/// resolução por host. Nunca guarda dado de negócio de nenhuma empresa.
/// </summary>
public class MasterDbContext : EFCoreStoreDbContext<FleetExecutiveTenantInfo>
{
    public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options)
    {
    }
}
