using FleetExecutive.Application.Clientes.Queries;
using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Clientes;
using MockQueryable.Moq;

namespace FleetExecutive.Application.Tests.Clientes;

public class GetCustomersQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _db = new();

    private void ComClientes(params Customer[] customers) =>
        _db.Setup(x => x.Customers).Returns(customers.ToList().BuildMockDbSet().Object);

    private static Customer Cliente(string nome, string email, bool ativo = true)
    {
        var c = Customer.Criar(TipoPessoa.Fisica, nome, email);
        if (!ativo)
        {
            c.Desativar();
        }

        return c;
    }

    [Fact]
    public async Task SemFiltro_RetornaTodosOrdenadosPorNome()
    {
        ComClientes(Cliente("Carlos", "c@example.com"), Cliente("Ana", "a@example.com"));

        var result = await new GetCustomersQueryHandler(_db.Object)
            .Handle(new GetCustomersQuery(null, null), CancellationToken.None);

        Assert.Equal(2, result.TotalRegistros);
        Assert.Collection(result.Items,
            item => Assert.Equal("Ana", item.Nome),
            item => Assert.Equal("Carlos", item.Nome));
    }

    [Fact]
    public async Task FiltroBusca_FiltraPorNome()
    {
        ComClientes(Cliente("Ana Maria", "ana@example.com"), Cliente("Carlos", "carlos@example.com"));

        var result = await new GetCustomersQueryHandler(_db.Object)
            .Handle(new GetCustomersQuery("ana", null), CancellationToken.None);

        Assert.Equal(1, result.TotalRegistros);
        Assert.Equal("Ana Maria", Assert.Single(result.Items).Nome);
    }

    [Fact]
    public async Task FiltroAtivoFalse_RetornaApenasInativos()
    {
        ComClientes(Cliente("Ativo", "ativo@example.com"), Cliente("Inativo", "inativo@example.com", ativo: false));

        var result = await new GetCustomersQueryHandler(_db.Object)
            .Handle(new GetCustomersQuery(null, Ativo: false), CancellationToken.None);

        Assert.Equal("Inativo", Assert.Single(result.Items).Nome);
    }
}
