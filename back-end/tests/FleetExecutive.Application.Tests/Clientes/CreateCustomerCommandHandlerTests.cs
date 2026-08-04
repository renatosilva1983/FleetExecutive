using FleetExecutive.Application.Clientes.Commands;
using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Clientes;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;

namespace FleetExecutive.Application.Tests.Clientes;

public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _db = new();

    private void ComClientes(params Customer[] customers) =>
        _db.Setup(x => x.Customers).Returns(customers.ToList().BuildMockDbSet().Object);

    private static CreateCustomerCommand Comando(string email = "novo@example.com") =>
        new(TipoPessoa.Fisica, "Cliente Novo", email, null, null, null, null);

    [Fact]
    public async Task EmailDuplicado_LancaInvalidOperationException()
    {
        var existente = Customer.Criar(TipoPessoa.Fisica, "Existente", "novo@example.com");
        ComClientes(existente);

        var handler = new CreateCustomerCommandHandler(_db.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(Comando(), CancellationToken.None));
    }

    [Fact]
    public async Task EmailNovo_AdicionaClienteEPersiste()
    {
        var customersSet = new List<Customer>().BuildMockDbSet();
        _db.Setup(x => x.Customers).Returns(customersSet.Object);
        _db.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreateCustomerCommandHandler(_db.Object);

        var id = await handler.Handle(Comando(), CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        customersSet.Verify(x => x.Add(It.Is<Customer>(c => c.Email == "novo@example.com")), Times.Once);
        _db.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
