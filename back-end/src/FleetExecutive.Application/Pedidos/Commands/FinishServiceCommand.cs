using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Pedidos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Pedidos.Commands;

public record FinishServiceCommand(Guid OrderItemId, string ChaveAcesso) : IRequest;

public class FinishServiceCommandValidator : AbstractValidator<FinishServiceCommand>
{
    public FinishServiceCommandValidator()
    {
        RuleFor(x => x.OrderItemId).NotEmpty();
        RuleFor(x => x.ChaveAcesso).NotEmpty();
    }
}

public class FinishServiceCommandHandler : IRequestHandler<FinishServiceCommand>
{
    private readonly IApplicationDbContext _db;

    public FinishServiceCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(FinishServiceCommand request, CancellationToken cancellationToken)
    {
        var item = await _db.OrderItems.FirstOrDefaultAsync(i => i.Id == request.OrderItemId, cancellationToken)
            ?? throw new KeyNotFoundException("Serviço não encontrado.");

        if (item.ChaveAcessoCheckin != request.ChaveAcesso)
        {
            throw new InvalidOperationException("Chave de acesso inválida.");
        }

        item.FinalizarServico();

        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == item.OrderId, cancellationToken);
        // Finalizado libera a Avaliação (NPS) — ver Estrutura/02-dominio-e-glossario.md, ciclo do pedido.
        order?.AvancarCiclo(StatusOperacional.Finalizado);

        _db.OrderAuditLogs.Add(OrderAuditLog.Criar(item.OrderId, null, TipoEventoAuditoria.FimServico,
            $"Serviço #{item.Id} finalizado."));

        await _db.SaveChangesAsync(cancellationToken);
    }
}
