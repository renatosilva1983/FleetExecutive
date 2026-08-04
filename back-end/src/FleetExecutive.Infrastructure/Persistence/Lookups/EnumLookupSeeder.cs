using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FleetExecutive.Infrastructure.Persistence.Lookups;

/// <summary>
/// Semeadura + reconciliação + validação das tabelas-catálogo, chamada na publicação (pelo
/// MigrationRunner), TUDO dentro de uma única transação:
///  - Semear: upsert idempotente dos valores do enum (roda N vezes sem duplicar, graças ao
///    ON CONFLICT — a chave é o próprio valor do enum).
///  - Reconciliar: remove linhas ÓRFÃS (existem no banco mas não no enum). Antes de apagar,
///    checa os vínculos (FKs de negócio): se alguma linha referencia a órfã, ABORTA com erro.
///  - Validar: garante que cada catálogo tem exatamente os valores esperados.
///
/// Atomicidade: se QUALQUER etapa falhar, o Commit não é alcançado e o Dispose faz ROLLBACK —
/// o banco volta ao estado anterior à rotina e o migrator sai != 0, barrando a subida da API.
/// (As migrations de schema ficam FORA desta transação: são aplicadas antes pelo Migrate(), que
/// tem transações próprias do EF e é rastreado/idempotente por natureza.)
/// </summary>
public static class EnumLookupSeeder
{
    public static void SeedAndValidate(DbContext db)
    {
        using var tx = db.Database.BeginTransaction();

        // 1) SEMEAR (idempotente): garante que todo valor do enum existe e está com o Nome atual.
        foreach (var t in EnumLookupRegistry.Tables)
        {
            var valores = string.Join(", ", t.Rows()
                .Select(r => $"({r.Id}, '{r.Nome.Replace("'", "''")}')"));
            // EF1002: o nome da tabela vem SEMPRE do EnumLookupRegistry (constante do código),
            // nunca de entrada externa — não há risco de injeção. Identificadores de tabela não
            // podem ser parametrizados, por isso a interpolação é intencional (vale para todo o arquivo).
#pragma warning disable EF1002
            db.Database.ExecuteSqlRaw(
                $"INSERT INTO {t.TableName} (\"Id\", \"Nome\") VALUES {valores} " +
                "ON CONFLICT (\"Id\") DO UPDATE SET \"Nome\" = EXCLUDED.\"Nome\";");
#pragma warning restore EF1002
        }

        // 2) RECONCILIAR: apagar linhas órfãs, mas só se nada de negócio as referenciar.
        foreach (var t in EnumLookupRegistry.Tables)
        {
            var idsValidos = string.Join(",", t.Rows().Select(r => r.Id));

#pragma warning disable EF1002
            var orfas = db.Database
                .SqlQueryRaw<int>($"SELECT \"Id\" AS \"Value\" FROM {t.TableName} WHERE \"Id\" NOT IN ({idsValidos})")
                .ToList();
#pragma warning restore EF1002
            if (orfas.Count == 0) continue;

            var idsOrfas = string.Join(",", orfas);

            // Checa cada FK de negócio que aponta para este catálogo. Se houver referência,
            // aborta (o rollback devolve tudo — inclusive os upserts do passo 1).
            foreach (var (childTable, childColumn) in ReferenciasDe(db, t))
            {
#pragma warning disable EF1002
                var referencias = db.Database
                    .SqlQueryRaw<int>($"SELECT COUNT(*)::int AS \"Value\" FROM {childTable} WHERE \"{childColumn}\" IN ({idsOrfas})")
                    .AsEnumerable().Single();
#pragma warning restore EF1002
                if (referencias > 0)
                    throw new InvalidOperationException(
                        $"Catálogo '{t.TableName}': não é possível remover o(s) valor(es) órfão(s) [{idsOrfas}] — " +
                        $"{referencias} referência(s) em {childTable}.\"{childColumn}\". Publicação abortada (rollback aplicado).");
            }

#pragma warning disable EF1002
            db.Database.ExecuteSqlRaw($"DELETE FROM {t.TableName} WHERE \"Id\" NOT IN ({idsValidos});");
#pragma warning restore EF1002
        }

        // 3) VALIDAR: cada catálogo deve ter EXATAMENTE os valores esperados.
        foreach (var t in EnumLookupRegistry.Tables)
        {
#pragma warning disable EF1002
            var encontrados = db.Database
                .SqlQueryRaw<int>($"SELECT COUNT(*)::int AS \"Value\" FROM {t.TableName}")
                .AsEnumerable().Single();
#pragma warning restore EF1002
            var esperados = t.Rows().Count;
            if (encontrados != esperados)
                throw new InvalidOperationException(
                    $"Catálogo '{t.TableName}': esperados {esperados} registros, encontrados {encontrados}.");
        }

        tx.Commit();
    }

    /// <summary>
    /// (tabela, coluna) de cada FK de negócio que aponta para o catálogo informado — derivado do
    /// EnumLookupRegistry.ForeignKeys (fonte única) e traduzido para os nomes reais no banco.
    /// </summary>
    private static IEnumerable<(string Table, string Column)> ReferenciasDe(DbContext db, EnumLookupDescriptor catalogo)
    {
        foreach (var (entity, lookup, property) in EnumLookupRegistry.ForeignKeys)
        {
            if (lookup != catalogo.ClrType) continue;

            var et = db.Model.FindEntityType(entity)
                     ?? throw new InvalidOperationException($"Entidade {entity.Name} não mapeada no modelo.");
            var table = et.GetTableName()!;
            var store = StoreObjectIdentifier.Table(table, et.GetSchema());
            var column = et.FindProperty(property)!.GetColumnName(store)!;
            yield return (table, column);
        }
    }
}
