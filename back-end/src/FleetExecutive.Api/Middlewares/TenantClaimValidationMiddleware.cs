using FleetExecutive.Application.Common.Interfaces;

namespace FleetExecutive.Api.Middlewares;

/// <summary>
/// Garante que o tenant resolvido pelo Host (Finbuckle) bate com a claim "tenant_id" do JWT do
/// usuário autenticado. Nunca confiar apenas na resolução por host nem apenas na claim
/// isoladamente — as duas fontes têm que concordar, senão 403. Ver
/// Estrutura/07-autenticacao-seguranca-rbac.md, seção "Anti-tampering", item 3.
/// </summary>
public class TenantClaimValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TenantClaimValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentTenant currentTenant, ICurrentUser currentUser)
    {
        if (currentUser.IsAuthenticated)
        {
            var tenantClaim = context.User.FindFirst("tenant_id")?.Value;
            var tenantClaimId = Guid.TryParse(tenantClaim, out var parsed) ? parsed : (Guid?)null;

            if (!currentTenant.IsResolved || tenantClaimId is null || tenantClaimId != currentTenant.TenantId)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Tenant do token não corresponde ao tenant resolvido pelo host.");
                return;
            }
        }

        await _next(context);
    }
}
