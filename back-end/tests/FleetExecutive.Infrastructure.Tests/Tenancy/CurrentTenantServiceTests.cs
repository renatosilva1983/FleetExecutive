using FleetExecutive.Infrastructure.Tenancy;
using Finbuckle.MultiTenant.Abstractions;

namespace FleetExecutive.Infrastructure.Tests.Tenancy;

public class CurrentTenantServiceTests
{
    private static CurrentTenantService ComTenant(FleetExecutiveTenantInfo? tenantInfo)
    {
        var context = new Mock<IMultiTenantContext>();
        context.SetupGet(c => c.TenantInfo).Returns(tenantInfo);

        var accessor = new Mock<IMultiTenantContextAccessor>();
        accessor.SetupGet(a => a.MultiTenantContext).Returns(context.Object);

        return new CurrentTenantService(accessor.Object);
    }

    [Fact]
    public void TenantResolvido_ExpoeIdIdentificadorEIsResolved()
    {
        var tenantId = Guid.NewGuid();
        var service = ComTenant(new FleetExecutiveTenantInfo
        {
            Id = tenantId.ToString(),
            Identifier = "acme",
        });

        Assert.True(service.IsResolved);
        Assert.Equal(tenantId, service.TenantId);
        Assert.Equal("acme", service.Identificador);
    }

    [Fact]
    public void SemTenant_NaoResolvido()
    {
        var service = ComTenant(null);

        Assert.False(service.IsResolved);
        Assert.Null(service.TenantId);
        Assert.Null(service.Identificador);
    }

    [Fact]
    public void IdNaoGuid_TenantIdRetornaNull()
    {
        var service = ComTenant(new FleetExecutiveTenantInfo
        {
            Id = "nao-e-guid",
            Identifier = "acme",
        });

        // Resolvido (TenantInfo != null), mas o Id não parseável vira TenantId nulo.
        Assert.True(service.IsResolved);
        Assert.Null(service.TenantId);
    }
}
