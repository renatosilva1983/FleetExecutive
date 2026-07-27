using System.Security.Claims;
using FleetExecutive.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FleetExecutive.Infrastructure.Identity;

/// <summary>
/// Implementação de ICurrentUser a partir das claims do JWT já validado pelo middleware de
/// autenticação (nunca de um campo do body/query — ver
/// Estrutura/07-autenticacao-seguranca-rbac.md, "Anti-tampering"). Claims esperadas: sub, perfil,
/// driver_id (só perfil Fornecedor), customer_id (só perfil Cliente).
/// </summary>
public class CurrentUserService : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public Guid? UserId =>
        Guid.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? User?.FindFirstValue("sub"), out var id)
            ? id
            : null;

    public string? Perfil => User?.FindFirstValue("perfil") ?? User?.FindFirstValue(ClaimTypes.Role);

    public Guid? DriverId => Guid.TryParse(User?.FindFirstValue("driver_id"), out var id) ? id : null;

    public Guid? CustomerId => Guid.TryParse(User?.FindFirstValue("customer_id"), out var id) ? id : null;
}
