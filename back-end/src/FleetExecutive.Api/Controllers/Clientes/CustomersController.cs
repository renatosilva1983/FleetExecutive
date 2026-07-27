using FleetExecutive.Application.Clientes.Commands;
using FleetExecutive.Application.Clientes.Queries;
using FleetExecutive.Domain.Clientes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetExecutive.Api.Controllers.Clientes;

/// <summary>
/// Módulo piloto do roadmap v1 (Estrutura/13-deploy-multitenant-e-roadmap.md) — prova tenancy +
/// RBAC + auditoria funcionando ponta a ponta. Matriz de permissões: Estrutura/08-perfis-e-permissoes.md
/// (Administrador=T, Operacional=E, Financeiro=V, Fornecedor/Cliente sem acesso amplo — Cliente
/// só veria o próprio cadastro, filtro "próprio" ainda não implementado aqui, ver TODO no handler).
/// </summary>
[ApiController]
[Route("api/v1/clientes")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Operacional,Financeiro")]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? busca, [FromQuery] bool? ativo,
        [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetCustomersQuery(busca, ativo, pagina, tamanhoPagina), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Administrador,Operacional,Financeiro")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustomerByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    public record CreateCustomerRequest(
        TipoPessoa Tipo, string Nome, string Email, string? CpfCnpj, string? Telefone,
        Guid? AtendenteId, IReadOnlyCollection<string>? Tags);

    [HttpPost]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> Create(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new CreateCustomerCommand(request.Tipo, request.Nome, request.Email,
            request.CpfCnpj, request.Telefone, request.AtendenteId, request.Tags), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    public record UpdateCustomerRequest(
        string Nome, string Email, string? CpfCnpj, string? Telefone, Guid? AtendenteId,
        string? Observacoes, IReadOnlyCollection<string>? Tags);

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> Update(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateCustomerCommand(id, request.Nome, request.Email, request.CpfCnpj,
            request.Telefone, request.AtendenteId, request.Observacoes, request.Tags), cancellationToken);
        return NoContent();
    }
}
