using FleetExecutive.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Prestadores.Commands;

/// <summary>
/// "Comissões em massa" — ação vista no sistema real (Estrutura/03-sistema-atual-analise.md,
/// tela de Motoristas). Toda alteração de comissão é uma ação sensível o bastante para caber no
/// padrão de auditoria transversal (Estrutura/06-modelo-de-dados.md) quando o módulo de
/// Comissões existir; por enquanto, o próprio MarkUpdated/CreatedBy da entidade já registra quem
/// mexeu e quando.
/// </summary>
public record BulkUpdateCommissionCommand(IReadOnlyCollection<Guid> DriverIds, decimal NovaComissaoPercentual) : IRequest<int>;

public class BulkUpdateCommissionCommandValidator : AbstractValidator<BulkUpdateCommissionCommand>
{
    public BulkUpdateCommissionCommandValidator()
    {
        RuleFor(x => x.DriverIds).NotEmpty();
        RuleFor(x => x.NovaComissaoPercentual).InclusiveBetween(0, 100);
    }
}

public class BulkUpdateCommissionCommandHandler : IRequestHandler<BulkUpdateCommissionCommand, int>
{
    private readonly IApplicationDbContext _db;

    public BulkUpdateCommissionCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<int> Handle(BulkUpdateCommissionCommand request, CancellationToken cancellationToken)
    {
        var drivers = await _db.Drivers
            .Where(d => request.DriverIds.Contains(d.Id))
            .ToListAsync(cancellationToken);

        foreach (var driver in drivers)
        {
            driver.AtualizarComissao(request.NovaComissaoPercentual);
        }

        await _db.SaveChangesAsync(cancellationToken);

        return drivers.Count;
    }
}
