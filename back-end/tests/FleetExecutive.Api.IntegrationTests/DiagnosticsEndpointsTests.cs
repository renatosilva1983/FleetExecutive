using System.Net;
using System.Text.Json;

namespace FleetExecutive.Api.IntegrationTests;

/// <summary>
/// Exercita o endpoint anônimo de diagnóstico ponta a ponta (DiagnosticsController.WhoAmI).
/// Prova que o pipeline completo responde: middleware de exceção, resolução multi-tenant por host,
/// rate limiter, autenticação/autorização e roteamento. Como o host de teste ("localhost") não
/// corresponde a nenhum tenant cadastrado, o tenant fica não-resolvido e o usuário não-autenticado.
/// </summary>
public class DiagnosticsEndpointsTests : IClassFixture<ApiTestFactory>
{
    private readonly ApiTestFactory _factory;

    public DiagnosticsEndpointsTests(ApiTestFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task WhoAmI_SemToken_Retorna200()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/diagnostics/whoami");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task WhoAmI_SemTenantResolvidoNemUsuario_RefleteEstadoNaoAutenticado()
    {
        var client = _factory.CreateClient();

        using var response = await client.GetAsync("/api/v1/diagnostics/whoami");
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var json = await JsonDocument.ParseAsync(stream);
        var root = json.RootElement;

        // Host "localhost" não bate com nenhum tenant → não resolvido.
        Assert.False(root.GetProperty("tenant").GetProperty("isResolved").GetBoolean());
        // Requisição anônima → usuário não autenticado.
        Assert.False(root.GetProperty("user").GetProperty("isAuthenticated").GetBoolean());
    }
}
