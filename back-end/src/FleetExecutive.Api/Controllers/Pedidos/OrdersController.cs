using FleetExecutive.Application.Pedidos.Commands;
using FleetExecutive.Application.Pedidos.Queries;
using FleetExecutive.Domain.Orcamentos;
using FleetExecutive.Domain.Pedidos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetExecutive.Api.Controllers.Pedidos;

/// <summary>
/// Matriz Estrutura/08-perfis-e-permissoes.md: Pedidos — Administrador=T, Operacional=E (sem
/// excluir/cancelar), Financeiro=V. Check-in/início/fim de serviço ficam em rotas próprias,
/// abertas por chave de acesso (perfil Fornecedor, sem conta completa — ver
/// Estrutura/10-modulos-comercial-operacional.md).
/// </summary>
[ApiController]
[Route("api/v1/pedidos")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Operacional,Financeiro")]
    public async Task<IActionResult> GetAll(
        [FromQuery] StatusComercial? statusComercial, [FromQuery] Guid? customerId, [FromQuery] Guid? atendenteId,
        [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 20, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetOrdersQuery(statusComercial, customerId, atendenteId, pagina, tamanhoPagina), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Administrador,Operacional,Financeiro")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    public record CreateOrderRequest(Guid CustomerId, OrigemOrcamento Origem, Guid? AtendenteId);

    [HttpPost]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> Create(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new CreateOrderCommand(request.CustomerId, request.Origem, request.AtendenteId), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    public record AddItemRequest(
        SubtipoServico Tipo, string Origem, string Destino, DateTimeOffset DataHoraIda, decimal ValorServico,
        DateTimeOffset? DataHoraVolta, Guid? DriverId, Guid? VehicleId);

    [HttpPost("{id:guid}/servicos")]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> AddItem(Guid id, AddItemRequest request, CancellationToken cancellationToken)
    {
        var itemId = await _mediator.Send(new AddOrderItemCommand(id, request.Tipo, request.Origem, request.Destino,
            request.DataHoraIda, request.ValorServico, request.DataHoraVolta, request.DriverId, request.VehicleId),
            cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id = itemId });
    }

    [HttpPost("{id:guid}/aceite-termos")]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> RegisterTermsAcceptance(Guid id, CancellationToken cancellationToken)
    {
        var codigo = await _mediator.Send(new RegisterTermsAcceptanceCommand(id), cancellationToken);
        return Ok(new { codigo });
    }

    public record AdvanceCycleRequest(StatusOperacional NovoStatus);

    [HttpPatch("{id:guid}/ciclo")]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> AdvanceCycle(Guid id, AdvanceCycleRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AdvanceOrderCycleCommand(id, request.NovoStatus), cancellationToken);
        return NoContent();
    }

    public record CancelRequest(string Motivo);

    [HttpPost("{id:guid}/cancelar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Cancel(Guid id, CancelRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelOrderCommand(id, request.Motivo), cancellationToken);
        return NoContent();
    }

    public record SurchargeRequest(decimal Valor, string Justificativa);

    [HttpPost("servicos/{itemId:guid}/acrescimo")]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> AddSurcharge(Guid itemId, SurchargeRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddSurchargeCommand(itemId, request.Valor, request.Justificativa), cancellationToken);
        return NoContent();
    }

    // --- Rotas do fluxo do prestador (perfil Fornecedor), abertas por chave de acesso ---

    public record CheckinRequest(string ChaveAcesso, double Lat, double Lng);

    [HttpPost("servicos/{itemId:guid}/checkin")]
    [AllowAnonymous]
    public async Task<IActionResult> Checkin(Guid itemId, CheckinRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RegisterCheckinCommand(itemId, request.ChaveAcesso, request.Lat, request.Lng), cancellationToken);
        return NoContent();
    }

    public record ChaveRequest(string ChaveAcesso);

    [HttpPost("servicos/{itemId:guid}/iniciar")]
    [AllowAnonymous]
    public async Task<IActionResult> StartService(Guid itemId, ChaveRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new StartServiceCommand(itemId, request.ChaveAcesso), cancellationToken);
        return NoContent();
    }

    [HttpPost("servicos/{itemId:guid}/finalizar")]
    [AllowAnonymous]
    public async Task<IActionResult> FinishService(Guid itemId, ChaveRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new FinishServiceCommand(itemId, request.ChaveAcesso), cancellationToken);
        return NoContent();
    }
}
