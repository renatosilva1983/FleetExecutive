using FleetExecutive.Application.Clientes.Dtos;
using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Clientes.Queries;

/// <summary>
/// Listagem paginada — ver Estrutura/03-sistema-atual-analise.md achado #9 (volume real grande).
/// Busca por nome/e-mail/CNPJ (ver Estrutura/11-modulos-cadastro.md).
/// </summary>
public record GetCustomersQuery(string? Busca, bool? Ativo, int Pagina = 1, int TamanhoPagina = 20)
    : IRequest<PaginatedList<CustomerListItemDto>>;

public class GetCustomersQueryValidator : AbstractValidator<GetCustomersQuery>
{
    public GetCustomersQueryValidator()
    {
        RuleFor(x => x.Pagina).GreaterThan(0);
        RuleFor(x => x.TamanhoPagina).InclusiveBetween(1, 100);
    }
}

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, PaginatedList<CustomerListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetCustomersQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public Task<PaginatedList<CustomerListItemDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Customers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Busca))
        {
            var termo = request.Busca.Trim().ToLowerInvariant();
            query = query.Where(c =>
                c.Nome.ToLower().Contains(termo) ||
                c.Email.ToLower().Contains(termo) ||
                (c.CpfCnpj != null && c.CpfCnpj.Contains(termo)));
        }

        if (request.Ativo is not null)
        {
            query = query.Where(c => c.Ativo == request.Ativo);
        }

        var projected = query
            .OrderBy(c => c.Nome)
            .Select(c => new CustomerListItemDto(c.Id, c.Nome, c.Tipo.ToString(), c.Ativo, c.Email, c.AtendenteId));

        return PaginatedList<CustomerListItemDto>.CreateAsync(projected, request.Pagina, request.TamanhoPagina, cancellationToken);
    }
}
