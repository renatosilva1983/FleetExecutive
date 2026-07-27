using FleetExecutive.Application.Financeiro.Commands;
using FleetExecutive.Application.Financeiro.Queries;
using FleetExecutive.Domain.Financeiro;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetExecutive.Api.Controllers.Financeiro;

/// <summary>Matriz Estrutura/08-perfis-e-permissoes.md: Cobranças/Faturas — Administrador=T, Financeiro=T.</summary>
[ApiController]
[Route("api/v1/cobrancas")]
[Authorize(Roles = "Administrador,Financeiro")]
public class ChargesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChargesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] StatusCobranca? status, [FromQuery] Guid? orderId,
        [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 20, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetChargesQuery(status, orderId, pagina, tamanhoPagina), cancellationToken);
        return Ok(result);
    }

    public record CreateChargeRequest(Guid OrderId, TipoCobranca Tipo, decimal Valor, DateOnly VenceEm);

    [HttpPost]
    public async Task<IActionResult> Create(CreateChargeRequest request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new CreateChargeCommand(request.OrderId, request.Tipo, request.Valor, request.VenceEm), cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id }, new { id });
    }

    public record RegisterPaymentRequest(bool RecebidoPelaEmpresa);

    [HttpPost("{id:guid}/registrar-recebimento")]
    public async Task<IActionResult> RegisterPayment(Guid id, RegisterPaymentRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RegisterChargePaymentCommand(id, request.RecebidoPelaEmpresa), cancellationToken);
        return NoContent();
    }
}
