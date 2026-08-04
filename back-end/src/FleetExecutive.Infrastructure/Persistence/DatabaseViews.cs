using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Infrastructure.Persistence;

/// <summary>
/// Views de leitura criadas/atualizadas na publicação (pelo MigrationRunner), de forma idempotente
/// via CREATE OR REPLACE — mesmo espírito do EnumLookupSeeder (SQL pós-migrate). Aqui vive a REGRA
/// de status de validade do documento (fonte única): o app LÊ o status já calculado pelo banco
/// (ver DriverDocumentView) em vez de reimplementar a regra em C#.
/// </summary>
public static class DatabaseViews
{
    public static void CreateOrReplace(DbContext db)
    {
        // Regra idêntica à antiga DriverDocument.Status (60 dias de alerta), agora única e no banco.
        // A data-base usa UTC para casar com o DateTime.UtcNow que a regra usava.
        db.Database.ExecuteSqlRaw("""
            CREATE OR REPLACE VIEW vw_driver_documents AS
            SELECT
                d."Id",
                d."DriverId",
                d."Tipo",
                d."Categoria",
                d."Numero",
                d."ValidoAte",
                CASE
                    WHEN d."ValidoAte" IS NULL                                          THEN 'SemValidade'
                    WHEN d."ValidoAte" <  (now() AT TIME ZONE 'UTC')::date              THEN 'Vencido'
                    WHEN d."ValidoAte" <= (now() AT TIME ZONE 'UTC')::date + 60         THEN 'VencendoEmBreve'
                    ELSE 'Valido'
                END AS "Status"
            FROM driver_documents d;
            """);
    }
}
