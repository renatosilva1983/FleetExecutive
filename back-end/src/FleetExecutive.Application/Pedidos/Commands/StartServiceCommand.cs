using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Pedidos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Pedidos.Commands;

/// <summary>
/// ChaveAcesso é validada aqui (não é regra de domínio) porque é um controle de acesso de API —
/// o mesmo padrão do check-in, permitindo o prestador operar sem conta completa (Estrutura/08-perfis-e-permissoes.md,
/// perfil Fornecedor).
/// </summary>
public record StartServiceCommand(Guid OrderItemId, string ChaveAcesso) : IRequest;

public class StartServiceCommandValidator : AbstractValidator<StartServiceCommand>
{
    public StartServiceCommandValidator()
    {
        RuleFor(x => x.OrderItemId).NotEmpty();
        RuleFor(x => x.ChaveAcesso).NotEmpty();
    }
}

public class StartServiceCommandHandler : IRequestHandler<StartServiceCommand>
{
    private readonly IApplicationDbContext _db;

    public StartServiceCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(StartServiceCommand request, CancellationToken cancellationToken)
    {
        var item = await _db.OrderItems.FirstOrDefaultAsync(i => i.Id == request.OrderItemId, cancellationToken)
            ?? throw new KeyNotFoundException("Serviço não encontrado.");

        if (item.ChaveAcessoCheckin != request.ChaveAcesso)
        {
            throw new InvalidOperationException("Chave de acesso inválida.");
        }

        item.IniciarServico();

        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == item.OrderId, cancellationToken);
        order?.AvancarCiclo(StatusOperacional.EmServico);

        _db.OrderAuditLogs.Add(OrderAuditLog.Criar(item.OrderId, null, TipoEventoAuditoria.InicioServico,
            $"Serviço #{item.Id} iniciado."));

        await _db.SaveChangesAsync(cancellationToken);
    }
}
