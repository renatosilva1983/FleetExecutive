using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Common.Models;
using FleetExecutive.Application.Financeiro.Dtos;
using FleetExecutive.Domain.Financeiro;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Financeiro.Queries;

public record GetChargesQuery(StatusCobranca? Status, Guid? OrderId, int Pagina = 1, int TamanhoPagina = 20)
    : IRequest<PaginatedList<ChargeDto>>;

public class GetChargesQueryValidator : AbstractValidator<GetChargesQuery>
{
    public GetChargesQueryValidator()
    {
        RuleFor(x => x.Pagina).GreaterThan(0);
        RuleFor(x => x.TamanhoPagina).InclusiveBetween(1, 200);
    }
}

public class GetChargesQueryHandler : IRequestHandler<GetChargesQuery, PaginatedList<ChargeDto>>
{
    private readonly IApplicationDbContext _db;

    public GetChargesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public Task<PaginatedList<ChargeDto>> Handle(GetChargesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Charges.AsNoTracking().AsQueryable();

        if (request.Status is not null)
        {
            query = query.Where(c => c.Status == request.Status);
        }

        if (request.OrderId is not null)
        {
            query = query.Where(c => c.OrderId == request.OrderId);
        }

        var projected = query
            .OrderBy(c => c.VenceEm)
            .Select(c => new ChargeDto(c.Id, c.OrderId, c.Tipo.ToString(), c.Status.ToString(), c.Origem.ToString(),
                c.Valor, c.VenceEm, c.PagoEm, c.ImpostoRetido));

        return PaginatedList<ChargeDto>.CreateAsync(projected, request.Pagina, request.TamanhoPagina, cancellationToken);
    }
}
