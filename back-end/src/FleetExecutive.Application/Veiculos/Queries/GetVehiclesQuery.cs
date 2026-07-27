using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Common.Models;
using FleetExecutive.Application.Veiculos.Dtos;
using FleetExecutive.Domain.Veiculos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Veiculos.Queries;

/// <summary>
/// Filtros conforme protótipo (Estrutura/04-prototipo-mapeamento-telas.md seção 3): placa/nº,
/// tipo, capacidade mínima, garagem, status.
/// </summary>
public record GetVehiclesQuery(
    string? Busca, TipoVeiculo? Tipo, int? CapacidadeMinima, Guid? GaragemId, StatusVeiculo? Status,
    int Pagina = 1, int TamanhoPagina = 20) : IRequest<PaginatedList<VehicleListItemDto>>;

public class GetVehiclesQueryValidator : AbstractValidator<GetVehiclesQuery>
{
    public GetVehiclesQueryValidator()
    {
        RuleFor(x => x.Pagina).GreaterThan(0);
        RuleFor(x => x.TamanhoPagina).InclusiveBetween(1, 100);
    }
}

public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, PaginatedList<VehicleListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetVehiclesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public Task<PaginatedList<VehicleListItemDto>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        var query =
            from v in _db.Vehicles.AsNoTracking()
            join f in _db.Fleets.AsNoTracking() on v.FleetId equals f.Id
            select new { Vehicle = v, Fleet = f };

        if (!string.IsNullOrWhiteSpace(request.Busca))
        {
            var termo = request.Busca.Trim().ToUpperInvariant();
            query = query.Where(x => x.Vehicle.Placa.Contains(termo) || x.Vehicle.NumeroOrdem.Contains(termo));
        }

        if (request.Tipo is not null)
        {
            query = query.Where(x => x.Fleet.Tipo == request.Tipo);
        }

        if (request.CapacidadeMinima is not null)
        {
            query = query.Where(x => x.Fleet.Capacidade >= request.CapacidadeMinima);
        }

        if (request.GaragemId is not null)
        {
            query = query.Where(x => x.Vehicle.GaragemId == request.GaragemId);
        }

        if (request.Status is not null)
        {
            query = query.Where(x => x.Vehicle.Status == request.Status);
        }

        var projected = query
            .OrderBy(x => x.Vehicle.NumeroOrdem)
            .Select(x => new VehicleListItemDto(
                x.Vehicle.Id, x.Vehicle.NumeroOrdem, x.Vehicle.Placa, x.Fleet.Titulo,
                x.Fleet.Tipo.ToString(), x.Fleet.Capacidade, x.Vehicle.Status.ToString(), x.Vehicle.GaragemId));

        return PaginatedList<VehicleListItemDto>.CreateAsync(projected, request.Pagina, request.TamanhoPagina, cancellationToken);
    }
}
