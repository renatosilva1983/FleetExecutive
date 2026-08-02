using FleetExecutive.Application.Usuarios.Commands;

namespace FleetExecutive.Application.Tests.Usuarios;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator = new();

    [Fact]
    public void CredenciaisValidas_Passa()
    {
        var result = _validator.Validate(new LoginCommand("user@example.com", "senha123", false));

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("", "senha123")]           // e-mail vazio
    [InlineData("nao-e-email", "senha123")] // e-mail malformado
    [InlineData("user@example.com", "")]    // senha vazia
    public void CredenciaisInvalidas_Falha(string email, string senha)
    {
        var result = _validator.Validate(new LoginCommand(email, senha, false));

        Assert.False(result.IsValid);
    }
}
