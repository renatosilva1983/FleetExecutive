using System.Net.Http.Headers;
using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Usuarios;
using FleetExecutive.Infrastructure.Persistence;
using FleetExecutive.Infrastructure.Tenancy;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FleetExecutive.Api.IntegrationTests;

/// <summary>
/// Hospeda a API em memória (TestServer) para os testes de integração, exercitando o pipeline real
/// — middlewares (exceção, multi-tenant, rate limiting), autenticação/autorização e roteamento.
///
/// Banco: usa a mesma configuração da aplicação (ver AddInfrastructure). A connection string vem da
/// variável de ambiente "StrCon_UserPost18" (ou de ConnectionStrings:Master no appsettings). Em
/// dev/local sem tenants cadastrados, o próprio banco master é o banco de negócio.
///
/// Para cobrir endpoints AUTENTICADOS, a fábrica semeia (idempotente) um tenant de teste no banco
/// master e um usuário Administrador no banco de negócio. As requisições autenticadas usam o host
/// <see cref="TenantHost"/> (para o Finbuckle resolver o tenant por host) e um JWT cujo claim
/// "tenant_id" bate com <see cref="TenantId"/> (exigência do TenantClaimValidationMiddleware).
/// </summary>
public class ApiTestFactory : WebApplicationFactory<Program>
{
    // Tenant de teste. O Identifier ("testco") é o rótulo que a estratégia de host do Finbuckle
    // ("__tenant__.*") extrai do primeiro segmento de TenantHost.
    public static readonly Guid TenantId = new("11111111-1111-1111-1111-111111111111");
    public const string TenantIdentifier = "testco";
    public const string TenantHost = "testco.localhost";

    // Credenciais do usuário Administrador semeado (usado no teste de login bem-sucedido).
    public const string AdminEmail = "admin.integration@testco.local";
    public const string AdminSenha = "SenhaTeste123!";

    // O banco é compartilhado entre instâncias da fábrica (mesmo Postgres). Semeia uma única vez
    // por processo de teste para evitar corrida entre classes de teste rodando em paralelo.
    private static readonly object SeedLock = new();
    private static bool _seeded;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Ambiente de teste (não "Development") — mantém Swagger desligado e evita depender de
        // configuração específica de desenvolvimento.
        builder.UseEnvironment("Testing");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        SeedTestData(host.Services);
        return host;
    }

    /// <summary>
    /// Cria um cliente HTTP autenticado como o perfil informado: define o host do tenant (para a
    /// resolução por host) e um Bearer token válido (tenant_id + role compatíveis).
    /// </summary>
    public HttpClient CreateAuthenticatedClient(Perfil perfil)
    {
        var client = CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri($"http://{TenantHost}"),
        });
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", GenerateToken(perfil));
        return client;
    }

    /// <summary>Cliente apontando para o host do tenant, porém sem token (para testes anônimos).</summary>
    public HttpClient CreateTenantClient() => CreateClient(new WebApplicationFactoryClientOptions
    {
        BaseAddress = new Uri($"http://{TenantHost}"),
    });

    private string GenerateToken(Perfil perfil)
    {
        using var scope = Services.CreateScope();
        var jwt = scope.ServiceProvider.GetRequiredService<IJwtTokenGenerator>();

        // Usuário transitório apenas para emitir o token — os endpoints autorizados por role não
        // consultam o usuário no banco, apenas validam tenant_id + role do token.
        var user = User.Criar($"Test {perfil}", $"{perfil}@testco.local".ToLowerInvariant(),
            "irrelevante", perfil);
        return jwt.GerarAccessToken(user, TenantId);
    }

    private static void SeedTestData(IServiceProvider services)
    {
        lock (SeedLock)
        {
            if (_seeded)
            {
                return;
            }

            using var scope = services.CreateScope();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var connectionString = config["StrCon_UserPost18"]
                ?? config.GetConnectionString("Master")
                ?? throw new InvalidOperationException(
                    "Connection string de teste não configurada (StrCon_UserPost18 ou ConnectionStrings:Master).");

            // 1) Tenant de teste no banco master (usado pela resolução por host do Finbuckle).
            var master = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
            var tenantSet = master.Set<FleetExecutiveTenantInfo>();
            if (!tenantSet.Any(t => t.Id == TenantIdentifierId))
            {
                tenantSet.Add(new FleetExecutiveTenantInfo
                {
                    Id = TenantIdentifierId,
                    Identifier = TenantIdentifier,
                    Name = "Empresa de Teste (Integração)",
                    ConnectionString = connectionString,
                });
                master.SaveChanges();
            }

            // 2) Usuário Administrador no banco de negócio do tenant.
            var tenant = new FleetExecutiveTenantInfo
            {
                Id = TenantIdentifierId,
                Identifier = TenantIdentifier,
                ConnectionString = connectionString,
            };
            var options = new DbContextOptionsBuilder<FleetExecutiveDbContext>()
                .UseNpgsql(connectionString)
                .Options;
            using var db = MultiTenantDbContext
                .Create<FleetExecutiveDbContext, FleetExecutiveTenantInfo>(tenant, options);

            if (!db.Users.Any(u => u.Email == AdminEmail))
            {
                var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
                db.Users.Add(User.Criar("Admin Integração", AdminEmail, hasher.Hash(AdminSenha),
                    Perfil.Administrador));
                db.SaveChanges();
            }

            _seeded = true;
        }
    }

    // TenantInfo.Id é string; guardamos o Guid em texto para o CurrentTenant conseguir parseá-lo.
    private static string TenantIdentifierId => TenantId.ToString();
}
