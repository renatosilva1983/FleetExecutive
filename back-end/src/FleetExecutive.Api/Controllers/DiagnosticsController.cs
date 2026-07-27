using FleetExecutive.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetExecutive.Api.Controllers;

/// <summary>
/// Endpoint de diagnóstico para provar, ponta a ponta, que a resolução de tenant por Host está
/// funcionando (ver Estrutura/13-deploy-multitenant-e-roadmap.md — v1, item "multi-tenancy
/// funcionando"). Não é um módulo de negócio — remover ou restringir a Administrador quando os
/// módulos reais tiverem endpoints equivalentes de health-check.
/// </summary>
[ApiController]
[Route("api/v1/diagnostics")]
public class DiagnosticsController : ControllerBase
{
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentUser _currentUser;

    public DiagnosticsController(ICurrentTenant currentTenant, ICurrentUser currentUser)
    {
        _currentTenant = currentTenant;
        _currentUser = currentUser;
    }

    [HttpGet("whoami")]
    [AllowAnonymous]
    public IActionResult WhoAmI()
    {
        return Ok(new
        {
            Tenant = new
            {
                _currentTenant.IsResolved,
                _currentTenant.TenantId,
                _currentTenant.Identificador,
            },
            User = new
            {
                _currentUser.IsAuthenticated,
                _currentUser.UserId,
                _currentUser.Perfil,
            }
        });
    }
}
