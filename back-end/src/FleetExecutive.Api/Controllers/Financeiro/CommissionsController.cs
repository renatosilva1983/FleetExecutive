using FleetExecutive.Application.Financeiro.Commands;
using FleetExecutive.Application.Financeiro.Queries;
using FleetExecutive.Domain.Financeiro;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetExecutive.Api.Controllers.Financeiro;

/// <summary>Matriz Estrutura/08-perfis-e-permissoes.md: Comissões — Administrador=T, Financeiro=T, demais sem acesso amplo (Fornecedor só veria as próprias — pendência de portal, ver ADRs anteriores).</summary>
[ApiController]
[Route("api/v1/comissoes")]
[Authorize(Roles = "Administrador,Financeiro")]
public class CommissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] TipoRecebedorComissao? recebedorTipo, [FromQuery] Guid? recebedorId,
        [FromQuery] StatusComissao? status, [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetCommissionsQuery(recebedorTipo, recebedorId, status, pagina, tamanhoPagina), cancellationToken);
        return Ok(result);
    }

    public record AdjustRequest(decimal NovoValor, string Justificativa);

    [HttpPatch("{id:guid}/ajustar")]
    public async Task<IActionResult> Adjust(Guid id, AdjustRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AdjustCommissionCommand(id, request.NovoValor, request.Justificativa), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/marcar-paga")]
    public async Task<IActionResult> MarkPaid(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new MarkCommissionPaidCommand(id), cancellationToken);
        return NoContent();
    }
}
