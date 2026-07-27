using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Prestadores;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Prestadores.Commands;

public record AddDriverLanguageCommand(Guid DriverId, string Idioma, NivelIdioma Nivel) : IRequest<Guid>;

public class AddDriverLanguageCommandValidator : AbstractValidator<AddDriverLanguageCommand>
{
    public AddDriverLanguageCommandValidator()
    {
        RuleFor(x => x.DriverId).NotEmpty();
        RuleFor(x => x.Idioma).NotEmpty().MaximumLength(60);
    }
}

public class AddDriverLanguageCommandHandler : IRequestHandler<AddDriverLanguageCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public AddDriverLanguageCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(AddDriverLanguageCommand request, CancellationToken cancellationToken)
    {
        var driverExiste = await _db.Drivers.AnyAsync(d => d.Id == request.DriverId, cancellationToken);
        if (!driverExiste)
        {
            throw new KeyNotFoundException("Prestador não encontrado.");
        }

        var idioma = DriverLanguage.Criar(request.DriverId, request.Idioma, request.Nivel);
        _db.DriverLanguages.Add(idioma);
        await _db.SaveChangesAsync(cancellationToken);

        return idioma.Id;
    }
}
