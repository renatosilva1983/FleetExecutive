using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Pedidos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Pedidos.Commands;

/// <summary>
/// Transições simples do ciclo operacional que não exigem dado adicional (Confirmado,
/// OrçamentoEnviado, "A caminho", Avaliação). Aceite de termos, check-in, início e fim de serviço
/// têm comandos dedicados por exigirem dados/validações próprias — ver
/// Estrutura/02-dominio-e-glossario.md para o ciclo completo de 8 passos.
/// </summary>
public record AdvanceOrderCycleCommand(Guid OrderId, StatusOperacional NovoStatus) : IRequest;

public class AdvanceOrderCycleCommandValidator : AbstractValidator<AdvanceOrderCycleCommand>
{
    public AdvanceOrderCycleCommandValidator() => RuleFor(x => x.OrderId).NotEmpty();
}

public class AdvanceOrderCycleCommandHandler : IRequestHandler<AdvanceOrderCycleCommand>
{
    private readonly IApplicationDbContext _db;

    public AdvanceOrderCycleCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(AdvanceOrderCycleCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException("Pedido não encontrado.");

        order.AvancarCiclo(request.NovoStatus);

        _db.OrderAuditLogs.Add(OrderAuditLog.Criar(order.Id, null, TipoEventoAuditoria.AlteracaoCampo,
            $"Ciclo do pedido avançou para {request.NovoStatus}.", campoAlterado: nameof(Order.StatusOperacional),
            valorNovo: request.NovoStatus.ToString()));

        await _db.SaveChangesAsync(cancellationToken);
    }
}
