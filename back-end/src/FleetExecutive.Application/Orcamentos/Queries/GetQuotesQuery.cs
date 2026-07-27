using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Common.Models;
using FleetExecutive.Application.Orcamentos.Dtos;
using FleetExecutive.Domain.Orcamentos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Orcamentos.Queries;

/// <summary>Base do Funil de Vendas (Kanban) e da listagem de Orçamentos — mesma query, agrupável por Status no frontend.</summary>
public record GetQuotesQuery(
    StatusFunil? Status, Guid? CustomerId, Guid? AtendenteId, int Pagina = 1, int TamanhoPagina = 20)
    : IRequest<PaginatedList<QuoteListItemDto>>;

public class GetQuotesQueryValidator : AbstractValidator<GetQuotesQuery>
{
    public GetQuotesQueryValidator()
    {
        RuleFor(x => x.Pagina).GreaterThan(0);
        RuleFor(x => x.TamanhoPagina).InclusiveBetween(1, 200);
    }
}

public class GetQuotesQueryHandler : IRequestHandler<GetQuotesQuery, PaginatedList<QuoteListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetQuotesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public Task<PaginatedList<QuoteListItemDto>> Handle(GetQuotesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Quotes.AsNoTracking().AsQueryable();

        if (request.Status is not null)
        {
            query = query.Where(q => q.Status == request.Status);
        }

        if (request.CustomerId is not null)
        {
            query = query.Where(q => q.CustomerId == request.CustomerId);
        }

        if (request.AtendenteId is not null)
        {
            query = query.Where(q => q.AtendenteId == request.AtendenteId);
        }

        var projected = query
            .OrderByDescending(q => q.CreatedAt)
            .Select(q => new QuoteListItemDto(q.Id, q.CustomerId, q.Origem.ToString(), q.Status.ToString(),
                q.DataServico, q.ValorEstimado, q.CreatedAt));

        return PaginatedList<QuoteListItemDto>.CreateAsync(projected, request.Pagina, request.TamanhoPagina, cancellationToken);
    }
}
