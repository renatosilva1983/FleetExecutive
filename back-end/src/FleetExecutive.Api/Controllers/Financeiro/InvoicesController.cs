using FleetExecutive.Application.Financeiro.Commands;
using FleetExecutive.Application.Financeiro.Queries;
using FleetExecutive.Domain.Financeiro;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetExecutive.Api.Controllers.Financeiro;

[ApiController]
[Route("api/v1/faturas")]
[Authorize(Roles = "Administrador,Financeiro")]
public class InvoicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvoicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] StatusFatura? status, [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetInvoicesQuery(status, pagina, tamanhoPagina), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{orderId:guid}/gerar")]
    public async Task<IActionResult> Generate(Guid orderId, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new GenerateInvoiceCommand(orderId), cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id }, new { id });
    }
}
