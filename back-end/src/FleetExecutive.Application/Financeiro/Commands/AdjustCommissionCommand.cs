using FleetExecutive.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Financeiro.Commands;

/// <summary>Réplica do padrão real (Estrutura/03-sistema-atual-analise.md achado #3) — alteração manual de comissão exige justificativa, marca "alterada".</summary>
public record AdjustCommissionCommand(Guid CommissionId, decimal NovoValor, string Justificativa) : IRequest;

public class AdjustCommissionCommandValidator : AbstractValidator<AdjustCommissionCommand>
{
    public AdjustCommissionCommandValidator()
    {
        RuleFor(x => x.CommissionId).NotEmpty();
        RuleFor(x => x.NovoValor).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Justificativa).NotEmpty();
    }
}

public class AdjustCommissionCommandHandler : IRequestHandler<AdjustCommissionCommand>
{
    private readonly IApplicationDbContext _db;

    public AdjustCommissionCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(AdjustCommissionCommand request, CancellationToken cancellationToken)
    {
        var commission = await _db.Commissions.FirstOrDefaultAsync(c => c.Id == request.CommissionId, cancellationToken)
            ?? throw new KeyNotFoundException("Comissão não encontrada.");

        commission.AjustarValor(request.NovoValor, request.Justificativa);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
