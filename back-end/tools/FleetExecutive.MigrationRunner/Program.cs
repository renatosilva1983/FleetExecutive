using FleetExecutive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// =============================================================================
//  MigrationRunner — aplica as migrations do banco "master" e encerra.
// -----------------------------------------------------------------------------
//  Roda como tarefa "run once" no docker-compose (serviço "migrator"): sobe,
//  garante que o schema do banco master (o catálogo de tenants — tabela
//  TenantInfo) esteja atualizado, e sai. Código de saída 0 = sucesso; a API só
//  sobe depois disso (ver depends_on: service_completed_successfully no compose).
//
//  IMPORTANTE — bancos POR-TENANT: como o modelo é database-per-tenant, os
//  bancos de cada tenant (FleetExecutiveDbContext) NÃO são migrados aqui. Cada
//  banco de tenant é migrado no momento em que o tenant é provisionado, usando a
//  connection string dele. Este runner cuida apenas do master.
// =============================================================================

// A connection string chega pela variável de ambiente ConnectionStrings__Master
// (definida no docker-compose.yml). O "__" é como o .NET mapeia seções aninhadas:
// ConnectionStrings__Master  ==  { "ConnectionStrings": { "Master": ... } }.
var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var masterConnectionString = configuration.GetConnectionString("Master")
    ?? throw new InvalidOperationException(
        "ConnectionStrings:Master não configurada. Defina a variável de ambiente " +
        "ConnectionStrings__Master (é o que o docker-compose.yml faz para o serviço migrator).");

var options = new DbContextOptionsBuilder<MasterDbContext>()
    .UseNpgsql(masterConnectionString)
    .Options;

Console.WriteLine("[migrator] Verificando migrations do banco master...");

try
{
    await using var db = new MasterDbContext(options);

    var pending = (await db.Database.GetPendingMigrationsAsync()).ToList();
    if (pending.Count == 0)
    {
        Console.WriteLine("[migrator] Banco master já está atualizado — nada a aplicar.");
        return 0;
    }

    Console.WriteLine($"[migrator] {pending.Count} migration(s) pendente(s): {string.Join(", ", pending)}");
    await db.Database.MigrateAsync();
    Console.WriteLine("[migrator] Migrations do master aplicadas com sucesso.");
    return 0;
}
catch (Exception ex)
{
    // Sair com código != 0 faz o serviço "migrator" falhar no compose, o que
    // impede a API de subir sobre um banco em estado inconsistente.
    Console.Error.WriteLine($"[migrator] FALHA ao aplicar migrations: {ex.Message}");
    Console.Error.WriteLine(ex);
    return 1;
}
