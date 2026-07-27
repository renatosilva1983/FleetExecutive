using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Common.Models;
using FleetExecutive.Application.Financeiro.Dtos;
using FleetExecutive.Domain.Financeiro;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Financeiro.Queries;

public record GetCommissionsQuery(
    TipoRecebedorComissao? RecebedorTipo, Guid? RecebedorId, StatusComissao? Status,
    int Pagina = 1, int TamanhoPagina = 20) : IRequest<PaginatedList<CommissionDto>>;

public class GetCommissionsQueryValidator : AbstractValidator<GetCommissionsQuery>
{
    public GetCommissionsQueryValidator()
    {
        RuleFor(x => x.Pagina).GreaterThan(0);
        RuleFor(x => x.TamanhoPagina).InclusiveBetween(1, 200);
    }
}

public class GetCommissionsQueryHandler : IRequestHandler<GetCommissionsQuery, PaginatedList<CommissionDto>>
{
    private readonly IApplicationDbContext _db;

    public GetCommissionsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public Task<PaginatedList<CommissionDto>> Handle(GetCommissionsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Commissions.AsNoTracking().AsQueryable();

        if (request.RecebedorTipo is not null)
        {
            query = query.Where(c => c.RecebedorTipo == request.RecebedorTipo);
        }

        if (request.RecebedorId is not null)
        {
            query = query.Where(c => c.RecebedorId == request.RecebedorId);
        }

        if (request.Status is not null)
        {
            query = query.Where(c => c.Status == request.Status);
        }

        var projected = query
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CommissionDto(c.Id, c.OrderItemId, c.Tipo.ToString(), c.RecebedorTipo.ToString(),
                c.RecebedorId, c.Percentual, c.Valor, c.Status.ToString(), c.PagoEm, c.Alterada, c.JustificativaAlteracao));

        return PaginatedList<CommissionDto>.CreateAsync(projected, request.Pagina, request.TamanhoPagina, cancellationToken);
    }
}
