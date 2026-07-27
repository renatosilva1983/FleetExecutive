using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Common.Models;
using FleetExecutive.Application.Pedidos.Dtos;
using FleetExecutive.Domain.Pedidos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Pedidos.Queries;

public record GetOrdersQuery(
    StatusComercial? StatusComercial, Guid? CustomerId, Guid? AtendenteId, int Pagina = 1, int TamanhoPagina = 20)
    : IRequest<PaginatedList<OrderListItemDto>>;

public class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
        RuleFor(x => x.Pagina).GreaterThan(0);
        RuleFor(x => x.TamanhoPagina).InclusiveBetween(1, 200);
    }
}

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PaginatedList<OrderListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetOrdersQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public Task<PaginatedList<OrderListItemDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Orders.AsNoTracking().AsQueryable();

        if (request.StatusComercial is not null)
        {
            query = query.Where(o => o.StatusComercial == request.StatusComercial);
        }

        if (request.CustomerId is not null)
        {
            query = query.Where(o => o.CustomerId == request.CustomerId);
        }

        if (request.AtendenteId is not null)
        {
            query = query.Where(o => o.AtendenteId == request.AtendenteId);
        }

        var projected = query
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderListItemDto(o.Id, o.CustomerId, o.Origem.ToString(), o.StatusComercial.ToString(),
                o.StatusOperacional.ToString(), o.ValorTotal, o.CreatedAt));

        return PaginatedList<OrderListItemDto>.CreateAsync(projected, request.Pagina, request.TamanhoPagina, cancellationToken);
    }
}
