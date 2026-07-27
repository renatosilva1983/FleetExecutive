using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Pedidos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Pedidos.Commands;

/// <summary>
/// Fluxo de check-in do prestador (Estrutura/10-modulos-comercial-operacional.md — chave de
/// acesso única por serviço + geolocalização). Endpoint pensado para o perfil Fornecedor
/// (Estrutura/08-perfis-e-permissoes.md), autenticado só pela chave, sem exigir conta completa.
/// </summary>
public record RegisterCheckinCommand(Guid OrderItemId, string ChaveAcesso, double Lat, double Lng) : IRequest;

public class RegisterCheckinCommandValidator : AbstractValidator<RegisterCheckinCommand>
{
    public RegisterCheckinCommandValidator()
    {
        RuleFor(x => x.OrderItemId).NotEmpty();
        RuleFor(x => x.ChaveAcesso).NotEmpty();
    }
}

public class RegisterCheckinCommandHandler : IRequestHandler<RegisterCheckinCommand>
{
    private readonly IApplicationDbContext _db;

    public RegisterCheckinCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(RegisterCheckinCommand request, CancellationToken cancellationToken)
    {
        var item = await _db.OrderItems.FirstOrDefaultAsync(i => i.Id == request.OrderItemId, cancellationToken)
            ?? throw new KeyNotFoundException("Serviço não encontrado.");

        item.RegistrarCheckin(request.ChaveAcesso, request.Lat, request.Lng);

        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == item.OrderId, cancellationToken);
        order?.AvancarCiclo(StatusOperacional.CheckIn);

        _db.OrderAuditLogs.Add(OrderAuditLog.Criar(item.OrderId, null, TipoEventoAuditoria.CheckinRegistrado,
            $"Check-in registrado para o serviço #{item.Id} em ({request.Lat}, {request.Lng})."));

        await _db.SaveChangesAsync(cancellationToken);
    }
}
