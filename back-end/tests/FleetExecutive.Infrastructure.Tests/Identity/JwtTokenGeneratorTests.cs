using System.IdentityModel.Tokens.Jwt;
using FleetExecutive.Domain.Usuarios;
using FleetExecutive.Infrastructure.Identity;
using Microsoft.Extensions.Configuration;

namespace FleetExecutive.Infrastructure.Tests.Identity;

public class JwtTokenGeneratorTests
{
    private const string Key = "chave-de-teste-super-secreta-com-mais-de-32-bytes";
    private const string Issuer = "FleetExecutive.Api";
    private const string Audience = "FleetExecutive.Frontend";

    private static JwtTokenGenerator CriarGerador()
    {
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["Jwt:Key"]).Returns(Key);
        config.Setup(c => c["Jwt:Issuer"]).Returns(Issuer);
        config.Setup(c => c["Jwt:Audience"]).Returns(Audience);
        return new JwtTokenGenerator(config.Object);
    }

    private static JwtSecurityToken Ler(string token) => new JwtSecurityTokenHandler().ReadJwtToken(token);

    [Fact]
    public void GerarAccessToken_IncluiClaimsMinimasEmissorEAudiencia()
    {
        var tenantId = Guid.NewGuid();
        var user = User.Criar("Fulano", "fulano@example.com", "hash", Perfil.Administrador);

        var jwt = Ler(CriarGerador().GerarAccessToken(user, tenantId));

        Assert.Equal(Issuer, jwt.Issuer);
        Assert.Contains(Audience, jwt.Audiences);
        Assert.Equal(user.Id.ToString(), jwt.Claims.Single(c => c.Type == "sub").Value);
        Assert.Equal(tenantId.ToString(), jwt.Claims.Single(c => c.Type == "tenant_id").Value);
        Assert.Equal(nameof(Perfil.Administrador), jwt.Claims.Single(c => c.Type == "perfil").Value);
    }

    [Fact]
    public void GerarAccessToken_PerfilFornecedor_IncluiDriverId()
    {
        var driverId = Guid.NewGuid();
        var user = User.Criar("Motorista", "motorista@example.com", "hash", Perfil.Fornecedor, driverId: driverId);

        var jwt = Ler(CriarGerador().GerarAccessToken(user, Guid.NewGuid()));

        Assert.Equal(driverId.ToString(), jwt.Claims.Single(c => c.Type == "driver_id").Value);
        Assert.DoesNotContain(jwt.Claims, c => c.Type == "customer_id");
    }

    [Fact]
    public void GerarAccessToken_PerfilCliente_IncluiCustomerId()
    {
        var customerId = Guid.NewGuid();
        var user = User.Criar("Cliente", "cliente@example.com", "hash", Perfil.Cliente, customerId: customerId);

        var jwt = Ler(CriarGerador().GerarAccessToken(user, Guid.NewGuid()));

        Assert.Equal(customerId.ToString(), jwt.Claims.Single(c => c.Type == "customer_id").Value);
        Assert.DoesNotContain(jwt.Claims, c => c.Type == "driver_id");
    }

    [Fact]
    public void GerarRefreshToken_RetornaValoresUnicosDeAltaEntropia()
    {
        var gerador = CriarGerador();

        var t1 = gerador.GerarRefreshToken();
        var t2 = gerador.GerarRefreshToken();

        Assert.NotEqual(t1, t2);
        Assert.Equal(64, t1.Length); // 32 bytes em hexadecimal
    }

    [Fact]
    public void GerarAccessToken_SemChaveConfigurada_LancaInvalidOperationException()
    {
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["Jwt:Key"]).Returns((string?)null);
        var gerador = new JwtTokenGenerator(config.Object);
        var user = User.Criar("Fulano", "fulano@example.com", "hash", Perfil.Administrador);

        Assert.Throws<InvalidOperationException>(() => gerador.GerarAccessToken(user, Guid.NewGuid()));
    }
}
