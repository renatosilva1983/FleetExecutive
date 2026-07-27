using FleetExecutive.Application.Prestadores.Commands;
using FleetExecutive.Application.Prestadores.Queries;
using FleetExecutive.Domain.Prestadores;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetExecutive.Api.Controllers.Prestadores;

/// <summary>
/// Matriz de permissões Estrutura/08-perfis-e-permissoes.md: Prestadores/Motoristas —
/// Administrador=T, Operacional=E (edita cadastro/capacidades, não comissão em massa nem
/// documentos sensíveis — restrito a Administrador), Financeiro=V (comissão/custo).
/// </summary>
[ApiController]
[Route("api/v1/prestadores")]
[Authorize]
public class DriversController : ControllerBase
{
    private readonly IMediator _mediator;

    public DriversController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Operacional,Financeiro")]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? busca, [FromQuery] TipoPrestador? tipo, [FromQuery] DriverCapability? capacidade,
        [FromQuery] bool? indicacao, [FromQuery] bool? ativo,
        [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetDriversQuery(busca, tipo, capacidade, indicacao, ativo, pagina, tamanhoPagina), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Administrador,Operacional,Financeiro")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDriverByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    public record CreateDriverRequest(
        string Nome, TipoPrestador Tipo, decimal ComissaoPercentual, string? Telefone, string? Referencia,
        bool Indicacao, string? FonteIndicacao, IReadOnlyCollection<DriverCapability>? Capacidades);

    [HttpPost]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> Create(CreateDriverRequest request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new CreateDriverCommand(request.Nome, request.Tipo,
            request.ComissaoPercentual, request.Telefone, request.Referencia, request.Indicacao,
            request.FonteIndicacao, request.Capacidades), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    public record UpdateDriverRequest(
        string Nome, string? Telefone, string? Referencia, bool Indicacao, string? FonteIndicacao,
        IReadOnlyCollection<DriverCapability>? Capacidades);

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> Update(Guid id, UpdateDriverRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateDriverCommand(id, request.Nome, request.Telefone, request.Referencia,
            request.Indicacao, request.FonteIndicacao, request.Capacidades), cancellationToken);
        return NoContent();
    }

    public record AddLanguageRequest(string Idioma, NivelIdioma Nivel);

    [HttpPost("{id:guid}/idiomas")]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> AddLanguage(Guid id, AddLanguageRequest request, CancellationToken cancellationToken)
    {
        var languageId = await _mediator.Send(new AddDriverLanguageCommand(id, request.Idioma, request.Nivel), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id = languageId });
    }

    public record AddDocumentRequest(TipoDocumentoPrestador Tipo, string? Categoria, string? Numero, DateOnly? ValidoAte);

    [HttpPost("{id:guid}/documentos")]
    [Authorize(Roles = "Administrador,Operacional")]
    public async Task<IActionResult> AddDocument(Guid id, AddDocumentRequest request, CancellationToken cancellationToken)
    {
        var documentId = await _mediator.Send(
            new AddDriverDocumentCommand(id, request.Tipo, request.Categoria, request.Numero, request.ValidoAte),
            cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id = documentId });
    }

    public record BulkCommissionRequest(IReadOnlyCollection<Guid> DriverIds, decimal NovaComissaoPercentual);

    [HttpPost("comissoes-em-massa")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> BulkUpdateCommission(BulkCommissionRequest request, CancellationToken cancellationToken)
    {
        var atualizados = await _mediator.Send(
            new BulkUpdateCommissionCommand(request.DriverIds, request.NovaComissaoPercentual), cancellationToken);
        return Ok(new { atualizados });
    }
}
