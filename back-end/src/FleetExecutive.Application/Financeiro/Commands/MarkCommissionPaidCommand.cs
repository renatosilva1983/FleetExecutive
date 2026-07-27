using FleetExecutive.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Financeiro.Commands;

public record MarkCommissionPaidCommand(Guid CommissionId) : IRequest;

public class MarkCommissionPaidCommandValidator : AbstractValidator<MarkCommissionPaidCommand>
{
    public MarkCommissionPaidCommandValidator() => RuleFor(x => x.CommissionId).NotEmpty();
}

public class MarkCommissionPaidCommandHandler : IRequestHandler<MarkCommissionPaidCommand>
{
    private readonly IApplicationDbContext _db;

    public MarkCommissionPaidCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(MarkCommissionPaidCommand request, CancellationToken cancellationToken)
    {
        var commission = await _db.Commissions.FirstOrDefaultAsync(c => c.Id == request.CommissionId, cancellationToken)
            ?? throw new KeyNotFoundException("Comissão não encontrada.");

        commission.MarcarPaga();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
