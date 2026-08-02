using System.Security.Claims;
using FleetExecutive.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;

namespace FleetExecutive.Infrastructure.Tests.Identity;

public class CurrentUserServiceTests
{
    private static CurrentUserService ComPrincipal(ClaimsPrincipal principal)
    {
        var context = new Mock<HttpContext>();
        context.SetupGet(c => c.User).Returns(principal);

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.SetupGet(a => a.HttpContext).Returns(context.Object);

        return new CurrentUserService(accessor.Object);
    }

    private static ClaimsPrincipal Autenticado(params Claim[] claims) =>
        new(new ClaimsIdentity(claims, authenticationType: "TestAuth"));

    [Fact]
    public void SemHttpContext_UsuarioNaoAutenticado()
    {
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.SetupGet(a => a.HttpContext).Returns((HttpContext?)null);

        var service = new CurrentUserService(accessor.Object);

        Assert.False(service.IsAuthenticated);
        Assert.Null(service.UserId);
        Assert.Null(service.Perfil);
    }

    [Fact]
    public void IdentidadeNaoAutenticada_IsAuthenticatedFalse()
    {
        var service = ComPrincipal(new ClaimsPrincipal(new ClaimsIdentity()));

        Assert.False(service.IsAuthenticated);
    }

    [Fact]
    public void ClaimsCompletas_ExpoePropriedades()
    {
        var userId = Guid.NewGuid();
        var driverId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var service = ComPrincipal(Autenticado(
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim("perfil", "Administrador"),
            new Claim("driver_id", driverId.ToString()),
            new Claim("customer_id", customerId.ToString())));

        Assert.True(service.IsAuthenticated);
        Assert.Equal(userId, service.UserId);
        Assert.Equal("Administrador", service.Perfil);
        Assert.Equal(driverId, service.DriverId);
        Assert.Equal(customerId, service.CustomerId);
    }

    [Fact]
    public void UserId_CaiParaClaimSub_QuandoNaoHaNameIdentifier()
    {
        var userId = Guid.NewGuid();

        var service = ComPrincipal(Autenticado(new Claim("sub", userId.ToString())));

        Assert.Equal(userId, service.UserId);
    }

    [Fact]
    public void Perfil_CaiParaClaimRole_QuandoNaoHaPerfil()
    {
        var service = ComPrincipal(Autenticado(new Claim(ClaimTypes.Role, "Financeiro")));

        Assert.Equal("Financeiro", service.Perfil);
    }

    [Fact]
    public void DriverIdECustomerId_AusentesOuInvalidos_RetornamNull()
    {
        var service = ComPrincipal(Autenticado(new Claim("driver_id", "nao-e-guid")));

        Assert.Null(service.DriverId);
        Assert.Null(service.CustomerId);
    }
}
