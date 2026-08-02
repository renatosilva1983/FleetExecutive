// =============================================================================
//  FleetExecutive.MigrationRunner — aplica migrations + semeia/valida catálogos.
// -----------------------------------------------------------------------------
//  Tarefa (roda uma vez e sai). No docker-compose, a API só sobe se este processo
//  terminar com código 0.
//
//  Fluxo:
//    A) migra o banco MASTER (tabela TenantInfo);
//    B) descobre os "alvos" de negócio:
//         - se houver tenants cadastrados -> um alvo por tenant (produção);
//         - se NÃO houver -> usa o próprio banco master como banco de negócio
//           local (dev), para o CI/local funcionar sem tenants cadastrados;
//    C) para cada alvo: migra o schema, SEMEIA as tabelas-catálogo (enums) de
//       forma idempotente e VALIDA que estão completas;
//    D) valida que não sobrou migration pendente.
//
//  Código de saída: 0 = ok; != 0 = falha (barra a subida da API).
// =============================================================================

using FleetExecutive.Infrastructure.Persistence;
using FleetExecutive.Infrastructure.Persistence.Lookups;
using FleetExecutive.Infrastructure.Tenancy;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

try
{
    var masterConnectionString = Environment.GetEnvironmentVariable("StrCon_UserPost18");
    if (string.IsNullOrWhiteSpace(masterConnectionString))
    {
        Console.Error.WriteLine("[migrator] StrCon_UserPost18 nao definida.");
        return 1;
    }

    // -------------------------------------------------------------------------
    //  PASSO A — migrar o banco MASTER.
    // -------------------------------------------------------------------------
    Console.WriteLine("[migrator] Migrando o banco MASTER...");
    var masterOptions = new DbContextOptionsBuilder<MasterDbContext>()
        .UseNpgsql(EnsureNpgsqlHost(masterConnectionString))
        .Options;

    List<FleetExecutiveTenantInfo> tenants;
    using (var master = new MasterDbContext(masterOptions))
    {
        master.Database.Migrate();
        GuardNoPendingMigrations(master, "master");
        tenants = master.Set<FleetExecutiveTenantInfo>().AsNoTracking().ToList();
    }

    // -------------------------------------------------------------------------
    //  PASSO B — montar a lista de alvos de negócio.
    // -------------------------------------------------------------------------
    List<FleetExecutiveTenantInfo> alvos;
    if (tenants.Count > 0)
    {
        alvos = tenants;
        Console.WriteLine($"[migrator] {tenants.Count} tenant(s) cadastrado(s).");
    }
    else
    {
        // Fallback dev/local: sem tenants, tratamos o próprio banco master como o
        // banco de negócio (setup de banco único em desenvolvimento).
        alvos = [new FleetExecutiveTenantInfo
        {
            Id = "local",
            Identifier = "local(dev)",
            ConnectionString = masterConnectionString,
        }];
        Console.WriteLine("[migrator] Nenhum tenant cadastrado — usando o banco master como banco de negócio local (dev).");
    }

    // -------------------------------------------------------------------------
    //  PASSO C — migrar + semear/validar cada alvo.
    // -------------------------------------------------------------------------
    foreach (var alvo in alvos)
    {
        if (string.IsNullOrWhiteSpace(alvo.ConnectionString))
        {
            Console.Error.WriteLine($"[migrator] Tenant '{alvo.Identifier}' sem ConnectionString.");
            return 1;
        }

        Console.WriteLine($"[migrator] Migrando '{alvo.Identifier}'...");
        var tenantOptions = new DbContextOptionsBuilder<FleetExecutiveDbContext>()
            .UseNpgsql(EnsureNpgsqlHost(alvo.ConnectionString))
            .Options;

        using var db = MultiTenantDbContext
            .Create<FleetExecutiveDbContext, FleetExecutiveTenantInfo>(alvo, tenantOptions);

        db.Database.Migrate();

        // Semeia (idempotente) e valida as tabelas-catálogo derivadas dos enums.
        Console.WriteLine($"[migrator]   semeando/validando catálogos de '{alvo.Identifier}'...");
        EnumLookupSeeder.SeedAndValidate(db);

        // PASSO D — nada pode ter ficado pendente.
        GuardNoPendingMigrations(db, $"tenant:{alvo.Identifier}");
    }

    Console.WriteLine("[migrator] Concluido com sucesso.");
    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"[migrator] FALHA: {ex.Message}");
    Console.Error.WriteLine(ex);
    return 1;
}

// =============================================================================
//  Funcoes auxiliares
// =============================================================================

// Dentro de contêiner, reescreve o Host para "host.docker.internal" (as imagens .NET definem
// DOTNET_RUNNING_IN_CONTAINER=true). A ÚLTIMA chave repetida vence numa connection string Npgsql,
// então basta acrescentar ";Host=..." no fim. Fora de contêiner (local), não mexe.
static string EnsureNpgsqlHost(string connectionString)
{
    var inContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
    return inContainer ? $"{connectionString};Host=host.docker.internal" : connectionString;
}

static void GuardNoPendingMigrations(DbContext db, string alvo)
{
    var pendentes = db.Database.GetPendingMigrations().ToList();
    if (pendentes.Count > 0)
        throw new InvalidOperationException(
            $"Migrations pendentes em {alvo}: {string.Join(", ", pendentes)}");
}
