using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Common.Models;

/// <summary>
/// Paginação server-side obrigatória em toda listagem de módulo com volume real grande (ver
/// Estrutura/03-sistema-atual-analise.md achado #9 — milhares de clientes/orçamentos).
/// </summary>
public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int Pagina { get; }
    public int TamanhoPagina { get; }
    public int TotalRegistros { get; }
    public int TotalPaginas => (int)Math.Ceiling(TotalRegistros / (double)TamanhoPagina);

    public PaginatedList(IReadOnlyCollection<T> items, int totalRegistros, int pagina, int tamanhoPagina)
    {
        Items = items;
        TotalRegistros = totalRegistros;
        Pagina = pagina;
        TamanhoPagina = tamanhoPagina;
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pagina, int tamanhoPagina,
        CancellationToken cancellationToken)
    {
        var total = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToListAsync(cancellationToken);
        return new PaginatedList<T>(items, total, pagina, tamanhoPagina);
    }
}
