using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Orcamentos;
using FleetExecutive.Domain.Pedidos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Orcamentos.Commands;

/// <summary>
/// Transição Orçamento → Pedido (Estrutura/10-modulos-comercial-operacional.md — "ao mover para
/// Reserva Confirmada, o sistema cria o order correspondente automaticamente"). Não copia preço
/// por serviço aqui (v2 é cálculo manual) — os order_items são adicionados depois via
/// AddOrderItemCommand, no fluxo normal de detalhamento do pedido.
/// </summary>
public record ConvertQuoteToOrderCommand(Guid QuoteId) : IRequest<Guid>;

public class ConvertQuoteToOrderCommandValidator : AbstractValidator<ConvertQuoteToOrderCommand>
{
    public ConvertQuoteToOrderCommandValidator()
    {
        RuleFor(x => x.QuoteId).NotEmpty();
    }
}

public class ConvertQuoteToOrderCommandHandler : IRequestHandler<ConvertQuoteToOrderCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public ConvertQuoteToOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(ConvertQuoteToOrderCommand request, CancellationToken cancellationToken)
    {
        var quote = await _db.Quotes.FirstOrDefaultAsync(q => q.Id == request.QuoteId, cancellationToken)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        var pedidoJaExiste = await _db.Orders.AnyAsync(o => o.QuoteId == quote.Id, cancellationToken);
        if (pedidoJaExiste)
        {
            throw new InvalidOperationException("Este orçamento já foi convertido em pedido.");
        }

        quote.AvancarFunil(StatusFunil.ReservaConfirmada);

        var order = Order.Criar(quote.CustomerId, quote.Origem, quote.AtendenteId, quote.Id);
        _db.Orders.Add(order);

        var log = OrderAuditLog.Criar(order.Id, quote.AtendenteId, TipoEventoAuditoria.Criado,
            $"Pedido criado a partir do orçamento #{quote.Id}.");
        _db.OrderAuditLogs.Add(log);

        await _db.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
