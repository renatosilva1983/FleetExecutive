using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Pedidos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Pedidos.Commands;

/// <summary>Restrito a Administrador (Estrutura/08-perfis-e-permissoes.md — cancelamento tem impacto financeiro e de auditoria).</summary>
public record CancelOrderCommand(Guid OrderId, string Motivo) : IRequest;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Motivo).NotEmpty();
    }
}

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
{
    private readonly IApplicationDbContext _db;

    public CancelOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException("Pedido não encontrado.");

        order.Cancelar(request.Motivo);

        _db.OrderAuditLogs.Add(OrderAuditLog.Criar(order.Id, null, TipoEventoAuditoria.Cancelado,
            $"Pedido cancelado. Motivo: {request.Motivo}", justificativa: request.Motivo));

        await _db.SaveChangesAsync(cancellationToken);
    }
}
