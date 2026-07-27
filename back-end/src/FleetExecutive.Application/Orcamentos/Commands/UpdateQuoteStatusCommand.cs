using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Orcamentos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Orcamentos.Commands;

public record UpdateQuoteStatusCommand(Guid QuoteId, StatusFunil NovoStatus, string? MotivoPerda) : IRequest;

public class UpdateQuoteStatusCommandValidator : AbstractValidator<UpdateQuoteStatusCommand>
{
    public UpdateQuoteStatusCommandValidator()
    {
        RuleFor(x => x.QuoteId).NotEmpty();
        RuleFor(x => x.MotivoPerda).NotEmpty().When(x => x.NovoStatus == StatusFunil.Perdido)
            .WithMessage("Motivo é obrigatório ao marcar um orçamento como perdido.");
    }
}

public class UpdateQuoteStatusCommandHandler : IRequestHandler<UpdateQuoteStatusCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateQuoteStatusCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateQuoteStatusCommand request, CancellationToken cancellationToken)
    {
        var quote = await _db.Quotes.FirstOrDefaultAsync(q => q.Id == request.QuoteId, cancellationToken)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        if (request.NovoStatus == StatusFunil.ReservaConfirmada)
        {
            throw new InvalidOperationException(
                "Use o endpoint de conversão em pedido para mover um orçamento para Reserva Confirmada.");
        }

        quote.AvancarFunil(request.NovoStatus, request.MotivoPerda);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
