using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Pedidos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Pedidos.Commands;

/// <summary>
/// Réplica o padrão real observado em produção (Estrutura/03-sistema-atual-analise.md achado
/// #3): toda alteração manual de valor exige justificativa textual, auditada.
/// </summary>
public record AddSurchargeCommand(Guid OrderItemId, decimal Valor, string Justificativa) : IRequest;

public class AddSurchargeCommandValidator : AbstractValidator<AddSurchargeCommand>
{
    public AddSurchargeCommandValidator()
    {
        RuleFor(x => x.OrderItemId).NotEmpty();
        RuleFor(x => x.Justificativa).NotEmpty();
    }
}

public class AddSurchargeCommandHandler : IRequestHandler<AddSurchargeCommand>
{
    private readonly IApplicationDbContext _db;

    public AddSurchargeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(AddSurchargeCommand request, CancellationToken cancellationToken)
    {
        var item = await _db.OrderItems.FirstOrDefaultAsync(i => i.Id == request.OrderItemId, cancellationToken)
            ?? throw new KeyNotFoundException("Serviço não encontrado.");

        item.AdicionarAcrescimo(request.Valor, request.Justificativa);

        var order = await _db.Orders.Include(o => o.Itens)
            .FirstOrDefaultAsync(o => o.Id == item.OrderId, cancellationToken);
        order?.RecalcularValorTotal();

        _db.OrderAuditLogs.Add(OrderAuditLog.Criar(item.OrderId, null, TipoEventoAuditoria.JustificativaAcrescimo,
            $"Acréscimo de {request.Valor:C} no serviço #{item.Id}. Justificativa: {request.Justificativa}",
            justificativa: request.Justificativa, valorNovo: request.Valor.ToString()));

        await _db.SaveChangesAsync(cancellationToken);
    }
}
