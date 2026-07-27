using FleetExecutive.Application.Orcamentos.Commands;
using FleetExecutive.Application.Orcamentos.Queries;
using FleetExecutive.Domain.Orcamentos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetExecutive.Api.Controllers.Orcamentos;

/// <summary>
/// Também serve de base para o Funil de Vendas (Kanban) — mesma listagem, agrupada por Status no
/// frontend. Matriz Estrutura/08-perfis-e-permissoes.md: Orçamentos/Funil — Administrador=T,
/// Operacional=T, Financeiro=V.
/// </summary>
[ApiController]
[Route("api/v1/orcamentos")]
[Authorize]
public class QuotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Operacional,Financeiro")]
    public async Task<IActionResult> GetAll(
        [FromQuery] StatusFunil? status, [FromQuery] Guid? customerId, [FromQuery] Guid? atendenteId,
        [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 20, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetQuotesQuery(status, customerId, atendenteId, pagina, tamanhoPagina), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Administrador,Operacional,Financeiro")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetQuoteByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> Create(CreateQuoteCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    public record UpdateStatusRequest(StatusFunil NovoStatus, string? MotivoPerda);

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateStatusRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateQuoteStatusCommand(id, request.NovoStatus, request.MotivoPerda), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/converter-em-pedido")]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> ConvertToOrder(Guid id, CancellationToken cancellationToken)
    {
        var orderId = await _mediator.Send(new ConvertQuoteToOrderCommand(id), cancellationToken);
        return Ok(new { orderId });
    }
}
