using FleetExecutive.Application.Clientes.Commands;
using FleetExecutive.Domain.Clientes;

namespace FleetExecutive.Application.Tests.Clientes;

public class CreateCustomerCommandValidatorTests
{
    private readonly CreateCustomerCommandValidator _validator = new();

    private static CreateCustomerCommand Valido(string nome = "Cliente Teste", string email = "cliente@example.com",
        string? cpfCnpj = null) =>
        new(TipoPessoa.Fisica, nome, email, cpfCnpj, null, null, null);

    [Fact]
    public void ComandoValido_Passa()
    {
        Assert.True(_validator.Validate(Valido()).IsValid);
    }

    [Fact]
    public void NomeVazio_Falha()
    {
        Assert.False(_validator.Validate(Valido(nome: "")).IsValid);
    }

    [Fact]
    public void EmailMalformado_Falha()
    {
        Assert.False(_validator.Validate(Valido(email: "nao-e-email")).IsValid);
    }

    [Fact]
    public void CpfCnpjMuitoLongo_Falha()
    {
        Assert.False(_validator.Validate(Valido(cpfCnpj: new string('9', 21))).IsValid);
    }
}
