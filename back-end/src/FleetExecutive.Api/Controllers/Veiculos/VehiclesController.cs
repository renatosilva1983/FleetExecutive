using FleetExecutive.Application.Veiculos.Commands;
using FleetExecutive.Application.Veiculos.Queries;
using FleetExecutive.Domain.Veiculos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetExecutive.Api.Controllers.Veiculos;

/// <summary>
/// Matriz de permissões Estrutura/08-perfis-e-permissoes.md: Veículos/Frotas — Administrador=T,
/// Operacional=V, demais perfis sem acesso.
/// </summary>
[ApiController]
[Route("api/v1/veiculos")]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? busca, [FromQuery] TipoVeiculo? tipo, [FromQuery] int? capacidadeMinima,
        [FromQuery] Guid? garagemId, [FromQuery] StatusVeiculo? status,
        [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetVehiclesQuery(busca, tipo, capacidadeMinima, garagemId, status, pagina, tamanhoPagina),
            cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVehicleByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    public record CreateVehicleRequest(
        Guid FleetId, string NumeroOrdem, string Placa, Guid? GaragemId, Guid? MotoristaHabitualId,
        IReadOnlyCollection<VehicleFeature>? Features);

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Create(CreateVehicleRequest request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new CreateVehicleCommand(request.FleetId, request.NumeroOrdem,
            request.Placa, request.GaragemId, request.MotoristaHabitualId, request.Features), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    public record UpdateStatusRequest(StatusVeiculo NovoStatus, string? Motivo, DateOnly? DisponivelDesde);

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateStatusRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateVehicleStatusCommand(id, request.NovoStatus, request.Motivo,
            request.DisponivelDesde), cancellationToken);
        return NoContent();
    }
}
