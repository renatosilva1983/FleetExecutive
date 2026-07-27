using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Veiculos.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Veiculos.Queries;

public record GetVehicleByIdQuery(Guid Id) : IRequest<VehicleDto>;

public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, VehicleDto>
{
    private readonly IApplicationDbContext _db;

    public GetVehicleByIdQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<VehicleDto> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _db.Vehicles.AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Veículo não encontrado.");

        return new VehicleDto(vehicle.Id, vehicle.FleetId, vehicle.NumeroOrdem, vehicle.Placa,
            vehicle.Status.ToString(), vehicle.GaragemId, vehicle.MotoristaHabitualId, vehicle.DisponivelDesde,
            vehicle.MotivoIndisponibilidade, vehicle.Features.Select(f => f.ToString()).ToList());
    }
}
