using System.Net;
using System.Net.Http.Json;

namespace FleetExecutive.Api.IntegrationTests;

/// <summary>
/// Garante que os controllers de negócio estão protegidos: sem token JWT, o endpoint de coleção
/// (GET na raiz de cada módulo) responde 401 Unauthorized. Cobre a exigência de autenticação do
/// pipeline (Estrutura/07-autenticacao-seguranca-rbac.md) para todos os módulos [Authorize].
/// </summary>
public class AuthorizationTests : IClassFixture<ApiTestFactory>
{
    private readonly ApiTestFactory _factory;

    public AuthorizationTests(ApiTestFactory factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/api/v1/clientes")]
    [InlineData("/api/v1/veiculos")]
    [InlineData("/api/v1/frotas")]
    [InlineData("/api/v1/prestadores")]
    [InlineData("/api/v1/pedidos")]
    [InlineData("/api/v1/orcamentos")]
    [InlineData("/api/v1/tarefas")]
    [InlineData("/api/v1/agenda")]
    [InlineData("/api/v1/faturas")]
    [InlineData("/api/v1/comissoes")]
    [InlineData("/api/v1/cobrancas")]
    public async Task Get_SemToken_Retorna401(string rota)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(rota);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ChangePassword_SemToken_Retorna401()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/v1/auth/change-password",
            new { senhaAtual = "qualquer", novaSenha = "OutraSenha123!" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
