using FleetExecutive.Application.Common.Models;
using MockQueryable;

namespace FleetExecutive.Application.Tests.Common;

public class PaginatedListTests
{
    private sealed record Item(int Valor);

    private static IQueryable<Item> Fonte(int quantidade) =>
        Enumerable.Range(1, quantidade).Select(i => new Item(i)).ToList().BuildMock();

    [Fact]
    public async Task CreateAsync_CalculaTotaisERetornaApenasAPagina()
    {
        // 25 itens, página 2 de tamanho 10 → deve trazer os itens 11..20.
        var page = await PaginatedList<Item>.CreateAsync(Fonte(25), pagina: 2, tamanhoPagina: 10, CancellationToken.None);

        Assert.Equal(25, page.TotalRegistros);
        Assert.Equal(3, page.TotalPaginas);
        Assert.Equal(2, page.Pagina);
        Assert.Equal(10, page.TamanhoPagina);
        Assert.Equal(Enumerable.Range(11, 10), page.Items.Select(i => i.Valor));
    }

    [Fact]
    public async Task CreateAsync_UltimaPaginaParcial_RetornaResto()
    {
        var page = await PaginatedList<Item>.CreateAsync(Fonte(25), pagina: 3, tamanhoPagina: 10, CancellationToken.None);

        Assert.Equal(5, page.Items.Count);
    }
}
