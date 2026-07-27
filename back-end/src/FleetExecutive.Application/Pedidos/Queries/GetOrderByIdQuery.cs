using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Pedidos.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Pedidos.Queries;

public record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto>;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IApplicationDbContext _db;

    public GetOrderByIdQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _db.Orders.AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Pedido não encontrado.");

        var itens = await _db.OrderItems.AsNoTracking()
            .Where(i => i.OrderId == request.Id)
            .ToListAsync(cancellationToken);

        var historico = await _db.OrderAuditLogs.AsNoTracking()
            .Where(l => l.OrderId == request.Id)
            .OrderByDescending(l => l.CriadoEm)
            .ToListAsync(cancellationToken);

        var itensDto = itens.Select(i => new OrderItemDto(i.Id, i.DriverId, i.VehicleId, i.Tipo.ToString(),
            i.Origem, i.Destino, i.DataHoraIda, i.DataHoraVolta, i.ValorServico, i.Acrescimo, i.Subtotal,
            i.ChaveAcessoCheckin, i.CheckinEm, i.InicioServicoEm, i.FimServicoEm)).ToList();

        var historicoDto = historico.Select(l => new OrderAuditLogDto(l.Id, l.AutorId, l.TipoEvento, l.Descricao,
            l.Justificativa, l.CriadoEm)).ToList();

        return new OrderDto(order.Id, order.QuoteId, order.CustomerId, order.AtendenteId, order.Origem.ToString(),
            order.StatusComercial.ToString(), order.StatusOperacional.ToString(), order.FormaPagamento,
            order.CodigoAceiteTermos, order.AceiteEm, order.ValorTotal, order.MotivoCancelamento, order.CreatedAt,
            itensDto, historicoDto);
    }
}
