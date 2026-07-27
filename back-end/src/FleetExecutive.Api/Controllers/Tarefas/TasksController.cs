using FleetExecutive.Application.Tarefas.Commands;
using FleetExecutive.Application.Tarefas.Queries;
using FleetExecutive.Domain.Tarefas;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetExecutive.Api.Controllers.Tarefas;

/// <summary>RBAC: Administrador e Operacional podem criar/editar tarefas (atividade operacional do dia a dia); Financeiro só visualiza.</summary>
[ApiController]
[Route("api/v1/tarefas")]
[Authorize(Roles = "Administrador,Operacional,Financeiro")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] StatusTarefa? status, [FromQuery] Guid? responsavelId, [FromQuery] VinculoTarefa? vinculoTipo,
        [FromQuery] Guid? vinculoId, [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 50,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetTasksQuery(status, responsavelId, vinculoTipo, vinculoId, pagina, tamanhoPagina), cancellationToken);
        return Ok(result);
    }

    public record CreateTaskRequest(
        string Titulo, PrioridadeTarefa Prioridade, Guid? ResponsavelId, string? Descricao,
        DateTimeOffset? Prazo, VinculoTarefa? VinculoTipo, Guid? VinculoId);

    [HttpPost]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> Create(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new CreateTaskCommand(request.Titulo, request.Prioridade,
            request.ResponsavelId, request.Descricao, request.Prazo, request.VinculoTipo, request.VinculoId),
            cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id }, new { id });
    }

    public record UpdateTaskRequest(
        string Titulo, string? Descricao, Guid? ResponsavelId, PrioridadeTarefa Prioridade, DateTimeOffset? Prazo);

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateTaskCommand(id, request.Titulo, request.Descricao, request.ResponsavelId,
            request.Prioridade, request.Prazo), cancellationToken);
        return NoContent();
    }

    public record UpdateStatusRequest(StatusTarefa NovoStatus);

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateStatusRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateTaskStatusCommand(id, request.NovoStatus), cancellationToken);
        return NoContent();
    }
}
