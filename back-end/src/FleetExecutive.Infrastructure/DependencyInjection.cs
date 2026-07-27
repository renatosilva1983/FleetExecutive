using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Infrastructure.Email;
using FleetExecutive.Infrastructure.Identity;
using FleetExecutive.Infrastructure.Persistence;
using FleetExecutive.Infrastructure.Tenancy;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FleetExecutive.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var masterConnectionString = configuration.GetConnectionString("Master")
            ?? throw new InvalidOperationException("ConnectionStrings:Master não configurada.");

        services.AddDbContext<MasterDbContext>(options => options.UseNpgsql(masterConnectionString));

        // Resolução de tenant por Host (ver Estrutura/ESTRUTURA-PROJETO.md seção 4.3). O cadastro
        // de tenants (FleetExecutiveTenantInfo) fica no MasterDbContext via WithEFCoreStore.
        services.AddMultiTenant<FleetExecutiveTenantInfo>()
            .WithHostStrategy()
            .WithEFCoreStore<MasterDbContext, FleetExecutiveTenantInfo>();

        // Banco por-tenant: a connection string é resolvida em runtime a partir do tenant
        // corrente (nunca fixa em appsettings — troca de banco em runtime por requisição).
        services.AddDbContext<FleetExecutiveDbContext>((sp, options) =>
        {
            var tenantInfo = sp.GetRequiredService<IMultiTenantContextAccessor<FleetExecutiveTenantInfo>>()
                .MultiTenantContext?.TenantInfo;
            options.UseNpgsql(tenantInfo?.ConnectionString ?? masterConnectionString);
        });

        services.AddScoped<ICurrentTenant, CurrentTenantService>();
        services.AddScoped<ICurrentUser, CurrentUserService>();

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<FleetExecutiveDbContext>());
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IEmailSender, NoOpEmailSender>();

        return services;
    }
}
