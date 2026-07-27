using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Veiculos.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Veiculos.Queries;

public record GetFleetsQuery(bool? Ativo) : IRequest<IReadOnlyCollection<FleetDto>>;

public class GetFleetsQueryHandler : IRequestHandler<GetFleetsQuery, IReadOnlyCollection<FleetDto>>
{
    private readonly IApplicationDbContext _db;

    public GetFleetsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyCollection<FleetDto>> Handle(GetFleetsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Fleets.AsNoTracking().AsQueryable();

        if (request.Ativo is not null)
        {
            query = query.Where(f => f.Ativo == request.Ativo);
        }

        return await query
            .OrderBy(f => f.Titulo)
            .Select(f => new FleetDto(f.Id, f.Titulo, f.Descricao, f.Tipo.ToString(), f.Categoria.ToString(),
                f.Capacidade, f.TemWc, f.TemAr, f.TemWifi, f.TemAntt, f.GaragemId, f.Ativo))
            .ToListAsync(cancellationToken);
    }
}
