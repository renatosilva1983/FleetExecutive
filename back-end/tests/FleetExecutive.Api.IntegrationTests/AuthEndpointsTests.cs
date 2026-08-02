using System.Net;
using System.Net.Http.Json;

namespace FleetExecutive.Api.IntegrationTests;

/// <summary>
/// Cobre os fluxos anônimos de autenticação (AuthController) que não dependem de dados semeados.
/// Exercita a Application layer via MediatR e o mapeamento de exceções para HTTP
/// (ExceptionHandlingMiddleware): credenciais inválidas → 401 (resposta genérica anti-enumeração),
/// e-mail malformado → 400 (validação).
///
/// Mantém poucas chamadas ao endpoint de login: ele usa a policy de rate limiting "auth"
/// (5 tentativas / 15 min por IP) e um excesso poderia disparar 429 e mascarar o teste.
/// </summary>
public class AuthEndpointsTests : IClassFixture<ApiTestFactory>
{
    private readonly ApiTestFactory _factory;

    public AuthEndpointsTests(ApiTestFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_CredenciaisInvalidas_Retorna401()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email = "inexistente@example.com", senha = "senha-errada", manterConectado = false });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_EmailMalformado_Retorna400()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email = "nao-e-email", senha = "qualquer", manterConectado = false });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
