using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Orcamentos;
using FleetExecutive.Domain.Pedidos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Pedidos.Commands;

/// <summary>Criação direta de pedido pelo painel, sem passar por um Orçamento (Estrutura/10-modulos-comercial-operacional.md).</summary>
public record CreateOrderCommand(Guid CustomerId, OrigemOrcamento Origem, Guid? AtendenteId) : IRequest<Guid>;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var clienteExiste = await _db.Customers.AnyAsync(c => c.Id == request.CustomerId, cancellationToken);
        if (!clienteExiste)
        {
            throw new KeyNotFoundException("Cliente não encontrado.");
        }

        var order = Order.Criar(request.CustomerId, request.Origem, request.AtendenteId);
        _db.Orders.Add(order);

        var log = OrderAuditLog.Criar(order.Id, request.AtendenteId, TipoEventoAuditoria.Criado,
            "Pedido criado manualmente pelo painel.");
        _db.OrderAuditLogs.Add(log);

        await _db.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
