using FleetExecutive.Application.Agenda.Dtos;
using FleetExecutive.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Agenda.Queries;

/// <summary>
/// Calendário/lista de disponibilidade (Estrutura/10-modulos-comercial-operacional.md). Filtros:
/// garagem, frota, veículo específico, período. Grupo de garagens/bloqueios administrativos ainda
/// não modelados (ver docs/adr/0004-modulo-veiculos-frotas.md) — período "ocioso" pode ser
/// derivado no frontend comparando o calendário com a ausência de reservas.
/// </summary>
public record GetVehicleAvailabilityQuery(
    DateOnly DataInicio, DateOnly DataFim, Guid? GaragemId, Guid? FleetId, Guid? VehicleId)
    : IRequest<IReadOnlyCollection<VehicleAvailabilityDto>>;

public class GetVehicleAvailabilityQueryValidator : AbstractValidator<GetVehicleAvailabilityQuery>
{
    public GetVehicleAvailabilityQueryValidator()
    {
        RuleFor(x => x.DataFim).GreaterThanOrEqualTo(x => x.DataInicio);
    }
}

public class GetVehicleAvailabilityQueryHandler
    : IRequestHandler<GetVehicleAvailabilityQuery, IReadOnlyCollection<VehicleAvailabilityDto>>
{
    private readonly IApplicationDbContext _db;

    public GetVehicleAvailabilityQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyCollection<VehicleAvailabilityDto>> Handle(GetVehicleAvailabilityQuery request,
        CancellationToken cancellationToken)
    {
        var veiculosQuery =
            from v in _db.Vehicles.AsNoTracking()
            join f in _db.Fleets.AsNoTracking() on v.FleetId equals f.Id
            select new { Vehicle = v, Fleet = f };

        if (request.GaragemId is not null)
        {
            veiculosQuery = veiculosQuery.Where(x => x.Vehicle.GaragemId == request.GaragemId);
        }

        if (request.FleetId is not null)
        {
            veiculosQuery = veiculosQuery.Where(x => x.Fleet.Id == request.FleetId);
        }

        if (request.VehicleId is not null)
        {
            veiculosQuery = veiculosQuery.Where(x => x.Vehicle.Id == request.VehicleId);
        }

        var veiculos = await veiculosQuery.ToListAsync(cancellationToken);
        var veiculoIds = veiculos.Select(x => x.Vehicle.Id).ToList();

        var inicioPeriodo = request.DataInicio.ToDateTime(TimeOnly.MinValue);
        var fimPeriodo = request.DataFim.ToDateTime(TimeOnly.MaxValue);

        var reservas = await _db.OrderItems.AsNoTracking()
            .Where(i => i.VehicleId != null && veiculoIds.Contains(i.VehicleId!.Value))
            .Where(i => i.DataHoraIda <= fimPeriodo && (i.DataHoraVolta ?? i.DataHoraIda) >= inicioPeriodo)
            .ToListAsync(cancellationToken);

        return veiculos.Select(x => new VehicleAvailabilityDto(
            x.Vehicle.Id, x.Vehicle.NumeroOrdem, x.Vehicle.Placa, x.Fleet.Titulo, x.Vehicle.GaragemId,
            x.Vehicle.Status.ToString(),
            reservas.Where(r => r.VehicleId == x.Vehicle.Id)
                .Select(r => new VehicleBookingDto(r.OrderId, r.Id, r.DataHoraIda, r.DataHoraVolta, r.Origem,
                    r.Destino, r.InicioServicoEm != null && r.FimServicoEm == null))
                .ToList())
        ).ToList();
    }
}
