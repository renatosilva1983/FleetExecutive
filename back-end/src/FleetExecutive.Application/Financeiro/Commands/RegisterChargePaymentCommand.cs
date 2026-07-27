using FleetExecutive.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Financeiro.Commands;

public record RegisterChargePaymentCommand(Guid ChargeId, bool RecebidoPelaEmpresa) : IRequest;

public class RegisterChargePaymentCommandValidator : AbstractValidator<RegisterChargePaymentCommand>
{
    public RegisterChargePaymentCommandValidator() => RuleFor(x => x.ChargeId).NotEmpty();
}

public class RegisterChargePaymentCommandHandler : IRequestHandler<RegisterChargePaymentCommand>
{
    private readonly IApplicationDbContext _db;

    public RegisterChargePaymentCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(RegisterChargePaymentCommand request, CancellationToken cancellationToken)
    {
        var charge = await _db.Charges.FirstOrDefaultAsync(c => c.Id == request.ChargeId, cancellationToken)
            ?? throw new KeyNotFoundException("Cobrança não encontrada.");

        charge.RegistrarRecebimento(request.RecebidoPelaEmpresa);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
