using FleetExecutive.Application.Agenda.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetExecutive.Api.Controllers.Agenda;

/// <summary>Matriz Estrutura/08-perfis-e-permissoes.md: Agenda de Veículos — Administrador=T, Operacional=E (leitura hoje, sem bloqueios ainda), Financeiro=V.</summary>
[ApiController]
[Route("api/v1/agenda")]
[Authorize(Roles = "Administrador,Operacional,Financeiro")]
public class AgendaController : ControllerBase
{
    private readonly IMediator _mediator;

    public AgendaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetDisponibilidade(
        [FromQuery] DateOnly dataInicio, [FromQuery] DateOnly dataFim, [FromQuery] Guid? garagemId,
        [FromQuery] Guid? fleetId, [FromQuery] Guid? vehicleId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetVehicleAvailabilityQuery(dataInicio, dataFim, garagemId, fleetId, vehicleId), cancellationToken);
        return Ok(result);
    }
}
