using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Financeiro;
using FleetExecutive.Domain.Pedidos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Financeiro.Commands;

/// <summary>Gera a fatura de um pedido "À Faturar" e marca o pedido como Concluído (Estrutura/12-modulos-financeiro-e-logging.md).</summary>
public record GenerateInvoiceCommand(Guid OrderId) : IRequest<Guid>;

public class GenerateInvoiceCommandValidator : AbstractValidator<GenerateInvoiceCommand>
{
    public GenerateInvoiceCommandValidator() => RuleFor(x => x.OrderId).NotEmpty();
}

public class GenerateInvoiceCommandHandler : IRequestHandler<GenerateInvoiceCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public GenerateInvoiceCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(GenerateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException("Pedido não encontrado.");

        if (order.StatusComercial != StatusComercial.AFaturar)
        {
            throw new InvalidOperationException("Só é possível gerar fatura para um pedido 'À Faturar'.");
        }

        var jaFaturado = await _db.Invoices.AnyAsync(i => i.OrderId == order.Id, cancellationToken);
        if (jaFaturado)
        {
            throw new InvalidOperationException("Este pedido já possui fatura gerada.");
        }

        var invoice = Invoice.Criar(order.Id);
        invoice.MarcarFaturado();
        _db.Invoices.Add(invoice);

        order.MarcarConcluido();

        await _db.SaveChangesAsync(cancellationToken);

        return invoice.Id;
    }
}
