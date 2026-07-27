using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Financeiro;
using FleetExecutive.Domain.Orcamentos;
using FleetExecutive.Domain.Pedidos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Pedidos.Commands;

public record AddOrderItemCommand(
    Guid OrderId, SubtipoServico Tipo, string Origem, string Destino, DateTimeOffset DataHoraIda,
    decimal ValorServico, DateTimeOffset? DataHoraVolta, Guid? DriverId, Guid? VehicleId) : IRequest<Guid>;

public class AddOrderItemCommandValidator : AbstractValidator<AddOrderItemCommand>
{
    public AddOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Origem).NotEmpty();
        RuleFor(x => x.Destino).NotEmpty();
        RuleFor(x => x.ValorServico).GreaterThanOrEqualTo(0);
    }
}

public class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, Guid>
{
    /// <summary>Taxa de comissão do atendente observada em produção (Estrutura/03-sistema-atual-analise.md achado #14) — ainda fixa, migra para system_settings quando o módulo de Configurações existir.</summary>
    private const decimal ComissaoAtendentePercentual = 2m;

    private readonly IApplicationDbContext _db;

    public AddOrderItemCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
    {
        // Include obrigatório: Order.RecalcularValorTotal soma a coleção _itens em memória — sem
        // o Include, um pedido que já tinha itens perderia o total dos itens anteriores.
        var order = await _db.Orders.Include(o => o.Itens)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException("Pedido não encontrado.");

        var item = OrderItem.Criar(order.Id, request.Tipo, request.Origem, request.Destino, request.DataHoraIda,
            request.ValorServico, request.DataHoraVolta, request.DriverId, request.VehicleId);

        order.AdicionarItem(item);
        _db.OrderItems.Add(item);

        // Geração automática de comissões (Estrutura/12-modulos-financeiro-e-logging.md) — réplica
        // do comportamento real observado em produção: toda vez que um serviço é adicionado, uma
        // comissão de atendente (taxa fixa) e, se houver prestador vinculado, uma comissão de
        // motorista (na % cadastrada no próprio Driver) são criadas.
        if (order.AtendenteId is not null)
        {
            _db.Commissions.Add(Commission.Criar(item.Id, TipoRecebedorComissao.Atendente,
                order.AtendenteId.Value, ComissaoAtendentePercentual, request.ValorServico));
        }

        if (request.DriverId is not null)
        {
            var driver = await _db.Drivers.FirstOrDefaultAsync(d => d.Id == request.DriverId, cancellationToken);
            if (driver is not null)
            {
                _db.Commissions.Add(Commission.Criar(item.Id, TipoRecebedorComissao.Motorista, driver.Id,
                    driver.ComissaoPercentual, request.ValorServico));
            }
        }

        await _db.SaveChangesAsync(cancellationToken);

        return item.Id;
    }
}
