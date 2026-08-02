using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FleetExecutive.Domain.Usuarios;

namespace FleetExecutive.Api.IntegrationTests;

/// <summary>
/// Cobre o caminho autenticado ponta a ponta: tenant resolvido por host + JWT válido + RBAC por
/// role + acesso ao banco do tenant. Usa o tenant e o usuário semeados pela <see cref="ApiTestFactory"/>.
/// </summary>
public class AuthenticatedEndpointsTests : IClassFixture<ApiTestFactory>
{
    private readonly ApiTestFactory _factory;

    public AuthenticatedEndpointsTests(ApiTestFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetClientes_ComTokenAdministrador_Retorna200()
    {
        var client = _factory.CreateAuthenticatedClient(Perfil.Administrador);

        var response = await client.GetAsync("/api/v1/clientes");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetFaturas_ComRoleSemPermissao_Retorna403()
    {
        // Faturas exige Administrador ou Financeiro; Operacional não tem acesso.
        var client = _factory.CreateAuthenticatedClient(Perfil.Operacional);

        var response = await client.GetAsync("/api/v1/faturas");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task WhoAmI_Autenticado_RefleteTenantResolvidoEUsuario()
    {
        var client = _factory.CreateAuthenticatedClient(Perfil.Administrador);

        using var response = await client.GetAsync("/api/v1/diagnostics/whoami");
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var json = await JsonDocument.ParseAsync(stream);
        var root = json.RootElement;

        Assert.True(root.GetProperty("tenant").GetProperty("isResolved").GetBoolean());
        Assert.Equal(ApiTestFactory.TenantIdentifier,
            root.GetProperty("tenant").GetProperty("identificador").GetString());
        Assert.True(root.GetProperty("user").GetProperty("isAuthenticated").GetBoolean());
        Assert.Equal(nameof(Perfil.Administrador),
            root.GetProperty("user").GetProperty("perfil").GetString());
    }

    [Fact]
    public async Task GetClientes_ComTokenMasHostSemTenant_Retorna403()
    {
        // Anti-tampering (TenantClaimValidationMiddleware): token com tenant_id, mas host que não
        // resolve nenhum tenant → as fontes não concordam → 403.
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", TokenAdministrador());

        var response = await client.GetAsync("/api/v1/clientes");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Login_CredenciaisValidasComHostDoTenant_RetornaAccessToken()
    {
        var client = _factory.CreateTenantClient();

        var response = await client.PostAsJsonAsync("/api/v1/auth/login", new
        {
            email = ApiTestFactory.AdminEmail,
            senha = ApiTestFactory.AdminSenha,
            manterConectado = false,
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var json = await JsonDocument.ParseAsync(stream);
        var accessToken = json.RootElement.GetProperty("accessToken").GetString();
        Assert.False(string.IsNullOrWhiteSpace(accessToken));
    }

    // Emite um token de Administrador reutilizando o gerador real via um cliente autenticado.
    private string TokenAdministrador()
    {
        using var authed = _factory.CreateAuthenticatedClient(Perfil.Administrador);
        return authed.DefaultRequestHeaders.Authorization!.Parameter!;
    }
}
