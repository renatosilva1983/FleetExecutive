using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Common.Models;
using FleetExecutive.Application.Financeiro.Dtos;
using FleetExecutive.Domain.Financeiro;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Financeiro.Queries;

public record GetInvoicesQuery(StatusFatura? Status, int Pagina = 1, int TamanhoPagina = 20)
    : IRequest<PaginatedList<InvoiceDto>>;

public class GetInvoicesQueryValidator : AbstractValidator<GetInvoicesQuery>
{
    public GetInvoicesQueryValidator()
    {
        RuleFor(x => x.Pagina).GreaterThan(0);
        RuleFor(x => x.TamanhoPagina).InclusiveBetween(1, 200);
    }
}

public class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, PaginatedList<InvoiceDto>>
{
    private readonly IApplicationDbContext _db;

    public GetInvoicesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public Task<PaginatedList<InvoiceDto>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Invoices.AsNoTracking().AsQueryable();

        if (request.Status is not null)
        {
            query = query.Where(i => i.Status == request.Status);
        }

        var projected = query
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new InvoiceDto(i.Id, i.OrderId, i.Status.ToString(), i.GeradoEm));

        return PaginatedList<InvoiceDto>.CreateAsync(projected, request.Pagina, request.TamanhoPagina, cancellationToken);
    }
}
