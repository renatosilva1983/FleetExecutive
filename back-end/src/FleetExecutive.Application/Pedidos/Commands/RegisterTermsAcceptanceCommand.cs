using System.Security.Cryptography;
using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Pedidos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Pedidos.Commands;

/// <summary>Aceite de termos com código simples (Estrutura/03-sistema-atual-analise.md achado #11 — sem assinatura digital complexa na v2).</summary>
public record RegisterTermsAcceptanceCommand(Guid OrderId) : IRequest<string>;

public class RegisterTermsAcceptanceCommandValidator : AbstractValidator<RegisterTermsAcceptanceCommand>
{
    public RegisterTermsAcceptanceCommandValidator() => RuleFor(x => x.OrderId).NotEmpty();
}

public class RegisterTermsAcceptanceCommandHandler : IRequestHandler<RegisterTermsAcceptanceCommand, string>
{
    private readonly IApplicationDbContext _db;

    public RegisterTermsAcceptanceCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<string> Handle(RegisterTermsAcceptanceCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException("Pedido não encontrado.");

        var codigo = RandomNumberGenerator.GetInt32(10_000_000, 99_999_999).ToString();
        order.RegistrarAceiteTermos(codigo);

        _db.OrderAuditLogs.Add(OrderAuditLog.Criar(order.Id, null, TipoEventoAuditoria.AceiteTermos,
            "Cliente aceitou os termos e condições do serviço."));

        await _db.SaveChangesAsync(cancellationToken);

        return codigo;
    }
}
