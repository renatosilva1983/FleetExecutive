using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Prestadores;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Prestadores.Commands;

public record AddDriverDocumentCommand(
    Guid DriverId, TipoDocumentoPrestador Tipo, string? Categoria, string? Numero, DateOnly? ValidoAte)
    : IRequest<Guid>;

public class AddDriverDocumentCommandValidator : AbstractValidator<AddDriverDocumentCommand>
{
    public AddDriverDocumentCommandValidator()
    {
        RuleFor(x => x.DriverId).NotEmpty();
        RuleFor(x => x.Categoria).NotEmpty().When(x => x.Tipo == TipoDocumentoPrestador.Cnh)
            .WithMessage("Categoria é obrigatória para documento do tipo CNH.");
    }
}

public class AddDriverDocumentCommandHandler : IRequestHandler<AddDriverDocumentCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public AddDriverDocumentCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(AddDriverDocumentCommand request, CancellationToken cancellationToken)
    {
        var driverExiste = await _db.Drivers.AnyAsync(d => d.Id == request.DriverId, cancellationToken);
        if (!driverExiste)
        {
            throw new KeyNotFoundException("Prestador não encontrado.");
        }

        var documento = DriverDocument.Criar(request.DriverId, request.Tipo, request.Categoria, request.Numero,
            request.ValidoAte);
        _db.DriverDocuments.Add(documento);
        await _db.SaveChangesAsync(cancellationToken);

        return documento.Id;
    }
}
