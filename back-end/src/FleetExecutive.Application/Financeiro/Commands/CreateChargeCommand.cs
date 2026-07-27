using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Financeiro;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Financeiro.Commands;

public record CreateChargeCommand(Guid OrderId, TipoCobranca Tipo, decimal Valor, DateOnly VenceEm) : IRequest<Guid>;

public class CreateChargeCommandValidator : AbstractValidator<CreateChargeCommand>
{
    public CreateChargeCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Valor).GreaterThan(0);
    }
}

public class CreateChargeCommandHandler : IRequestHandler<CreateChargeCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateChargeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateChargeCommand request, CancellationToken cancellationToken)
    {
        var orderExiste = await _db.Orders.AnyAsync(o => o.Id == request.OrderId, cancellationToken);
        if (!orderExiste)
        {
            throw new KeyNotFoundException("Pedido não encontrado.");
        }

        var charge = Charge.Criar(request.OrderId, request.Tipo, request.Valor, request.VenceEm);
        _db.Charges.Add(charge);
        await _db.SaveChangesAsync(cancellationToken);

        return charge.Id;
    }
}
