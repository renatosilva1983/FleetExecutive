using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Infrastructure.Persistence.Lookups;

/// <summary>
/// Semeadura + validação das tabelas-catálogo, chamada na publicação (pelo MigrationRunner).
///  - Semear: upsert idempotente dos valores do enum (roda N vezes sem duplicar, graças ao
///    ON CONFLICT — a chave é o próprio valor do enum).
///  - Validar: garante que cada catálogo tem, no mínimo, todos os valores esperados; se faltar,
///    lança e o migrator sai com código != 0, barrando a subida da API.
/// </summary>
public static class EnumLookupSeeder
{
    public static void SeedAndValidate(DbContext db)
    {
        // 1) Semear (idempotente).
        foreach (var t in EnumLookupRegistry.Tables)
        {
            var valores = string.Join(", ", t.Rows().Select(r => $"({r.Id}, '{r.Nome}')"));
            db.Database.ExecuteSqlRaw(
                $"INSERT INTO {t.TableName} (\"Id\", \"Nome\") VALUES {valores} " +
                "ON CONFLICT (\"Id\") DO UPDATE SET \"Nome\" = EXCLUDED.\"Nome\";");
        }

        // 2) Validar (barra o deploy se algum catálogo estiver incompleto).
        foreach (var t in EnumLookupRegistry.Tables)
        {
            // EF1002: o nome da tabela vem SEMPRE do EnumLookupRegistry (constante do código),
            // nunca de entrada externa — não há risco de injeção. Identificadores de tabela não
            // podem ser parametrizados, por isso a interpolação é intencional.
#pragma warning disable EF1002
            var encontrados = db.Database
                .SqlQueryRaw<int>($"SELECT COUNT(*)::int AS \"Value\" FROM {t.TableName}")
                .AsEnumerable().Single();
#pragma warning restore EF1002
            var esperados = t.Rows().Count;
            if (encontrados < esperados)
                throw new InvalidOperationException(
                    $"Catálogo '{t.TableName}': esperados {esperados} registros, encontrados {encontrados}.");
        }
    }
}
