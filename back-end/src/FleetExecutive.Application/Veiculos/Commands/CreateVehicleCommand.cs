using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Veiculos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Veiculos.Commands;

public record CreateVehicleCommand(
    Guid FleetId, string NumeroOrdem, string Placa, Guid? GaragemId, Guid? MotoristaHabitualId,
    IReadOnlyCollection<VehicleFeature>? Features) : IRequest<Guid>;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(x => x.FleetId).NotEmpty();
        RuleFor(x => x.NumeroOrdem).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Placa).NotEmpty().MaximumLength(10);
    }
}

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateVehicleCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var fleetExiste = await _db.Fleets.AnyAsync(f => f.Id == request.FleetId, cancellationToken);
        if (!fleetExiste)
        {
            throw new KeyNotFoundException("Frota (categoria de veículo) não encontrada.");
        }

        var placaNormalizada = request.Placa.Trim().ToUpperInvariant();
        var placaDuplicada = await _db.Vehicles.AnyAsync(v => v.Placa == placaNormalizada, cancellationToken);
        if (placaDuplicada)
        {
            throw new InvalidOperationException("Já existe um veículo cadastrado com esta placa.");
        }

        var vehicle = Vehicle.Criar(request.FleetId, request.NumeroOrdem, request.Placa, request.GaragemId,
            request.MotoristaHabitualId);

        if (request.Features is { Count: > 0 })
        {
            vehicle.DefinirFeatures(request.Features);
        }

        _db.Vehicles.Add(vehicle);
        await _db.SaveChangesAsync(cancellationToken);

        return vehicle.Id;
    }
}
