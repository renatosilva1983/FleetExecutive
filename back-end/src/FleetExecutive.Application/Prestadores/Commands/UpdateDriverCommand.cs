using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Prestadores;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Prestadores.Commands;

public record UpdateDriverCommand(
    Guid Id, string Nome, string? Telefone, string? Referencia, bool Indicacao, string? FonteIndicacao,
    IReadOnlyCollection<DriverCapability>? Capacidades) : IRequest;

public class UpdateDriverCommandValidator : AbstractValidator<UpdateDriverCommand>
{
    public UpdateDriverCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
    }
}

public class UpdateDriverCommandHandler : IRequestHandler<UpdateDriverCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateDriverCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = await _db.Drivers.FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Prestador não encontrado.");

        driver.Atualizar(request.Nome, request.Telefone, request.Referencia, request.Indicacao, request.FonteIndicacao);

        if (request.Capacidades is not null)
        {
            driver.DefinirCapacidades(request.Capacidades);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
