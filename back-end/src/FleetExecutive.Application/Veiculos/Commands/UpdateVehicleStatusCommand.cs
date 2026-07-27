using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Veiculos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Veiculos.Commands;

/// <summary>
/// Transições de status administrativo do veículo (Estrutura/11-modulos-cadastro.md). "Disponível"
/// aceita uma data opcional de retorno; "Manutenção" exige motivo (mesmo padrão de auditoria
/// textual usado em Comissões/Pedidos — ver Estrutura/06-modelo-de-dados.md).
/// </summary>
public record UpdateVehicleStatusCommand(Guid VehicleId, StatusVeiculo NovoStatus, string? Motivo,
    DateOnly? DisponivelDesde) : IRequest;

public class UpdateVehicleStatusCommandValidator : AbstractValidator<UpdateVehicleStatusCommand>
{
    public UpdateVehicleStatusCommandValidator()
    {
        RuleFor(x => x.VehicleId).NotEmpty();
        RuleFor(x => x.Motivo).NotEmpty().When(x => x.NovoStatus == StatusVeiculo.Manutencao)
            .WithMessage("Motivo é obrigatório ao marcar um veículo em manutenção.");
    }
}

public class UpdateVehicleStatusCommandHandler : IRequestHandler<UpdateVehicleStatusCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateVehicleStatusCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateVehicleStatusCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == request.VehicleId, cancellationToken)
            ?? throw new KeyNotFoundException("Veículo não encontrado.");

        switch (request.NovoStatus)
        {
            case StatusVeiculo.Manutencao:
                vehicle.MarcarEmManutencao(request.Motivo!);
                break;
            case StatusVeiculo.Ativo:
                vehicle.MarcarDisponivel(request.DisponivelDesde);
                break;
            case StatusVeiculo.Inativo:
                vehicle.Inativar();
                break;
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
