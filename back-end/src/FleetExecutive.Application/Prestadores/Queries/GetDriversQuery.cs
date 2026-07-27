using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Common.Models;
using FleetExecutive.Application.Prestadores.Dtos;
using FleetExecutive.Domain.Prestadores;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Prestadores.Queries;

/// <summary>
/// Filtros do protótipo que dependem de histórico de pedidos (rota/região, disponibilidade em
/// data) ficam para quando o módulo Pedidos existir — ver Estrutura/11-modulos-cadastro.md.
/// </summary>
public record GetDriversQuery(
    string? Busca, TipoPrestador? Tipo, DriverCapability? Capacidade, bool? Indicacao, bool? Ativo,
    int Pagina = 1, int TamanhoPagina = 20) : IRequest<PaginatedList<DriverListItemDto>>;

public class GetDriversQueryValidator : AbstractValidator<GetDriversQuery>
{
    public GetDriversQueryValidator()
    {
        RuleFor(x => x.Pagina).GreaterThan(0);
        RuleFor(x => x.TamanhoPagina).InclusiveBetween(1, 100);
    }
}

public class GetDriversQueryHandler : IRequestHandler<GetDriversQuery, PaginatedList<DriverListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetDriversQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public Task<PaginatedList<DriverListItemDto>> Handle(GetDriversQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Drivers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Busca))
        {
            var termo = request.Busca.Trim().ToLowerInvariant();
            query = query.Where(d =>
                d.Nome.ToLower().Contains(termo) ||
                (d.Referencia != null && d.Referencia.ToLower().Contains(termo)) ||
                (d.Telefone != null && d.Telefone.Contains(termo)));
        }

        if (request.Tipo is not null)
        {
            query = query.Where(d => d.Tipo == request.Tipo);
        }

        if (request.Indicacao is not null)
        {
            query = query.Where(d => d.Indicacao == request.Indicacao);
        }

        if (request.Ativo is not null)
        {
            query = query.Where(d => d.Ativo == request.Ativo);
        }

        if (request.Capacidade is not null)
        {
            // Capacidades é persistida como JSON serializado (campo privado "_capacidades", ver
            // DriverConfiguration) — não é uma coluna relacional pesquisável por .Contains() sobre
            // a lista tipada. Como os nomes dos enums não colidem como substring uns dos outros,
            // uma busca textual no JSON serializado é suficiente e traduzível para SQL (LIKE).
            var nomeCapacidade = request.Capacidade.Value.ToString();
            query = query.Where(d => EF.Property<string>(d, "_capacidades").Contains(nomeCapacidade));
        }

        var projected = query
            .OrderBy(d => d.Nome)
            .Select(d => new DriverListItemDto(d.Id, d.Nome, d.Tipo.ToString(), d.Ativo, d.Referencia,
                d.Telefone, d.ComissaoPercentual, d.Indicacao));

        return PaginatedList<DriverListItemDto>.CreateAsync(projected, request.Pagina, request.TamanhoPagina, cancellationToken);
    }
}
