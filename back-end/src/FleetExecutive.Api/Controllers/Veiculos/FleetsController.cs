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
[Route("api/v1/frotas")]
[Authorize]
public class FleetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FleetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> GetAll([FromQuery] bool? ativo, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFleetsQuery(ativo), cancellationToken);
        return Ok(result);
    }

    public record CreateFleetRequest(
        string Titulo, TipoVeiculo Tipo, CategoriaVeiculo Categoria, int Capacidade, string? Descricao,
        bool TemWc, bool TemAr, bool TemWifi, bool TemAntt, Guid? GaragemId);

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Create(CreateFleetRequest request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new CreateFleetCommand(request.Titulo, request.Tipo, request.Categoria,
            request.Capacidade, request.Descricao, request.TemWc, request.TemAr, request.TemWifi,
            request.TemAntt, request.GaragemId), cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id }, new { id });
    }
}
