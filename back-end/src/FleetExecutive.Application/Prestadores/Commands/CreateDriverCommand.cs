using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Prestadores;
using FluentValidation;
using MediatR;

namespace FleetExecutive.Application.Prestadores.Commands;

public record CreateDriverCommand(
    string Nome, TipoPrestador Tipo, decimal ComissaoPercentual, string? Telefone, string? Referencia,
    bool Indicacao, string? FonteIndicacao, IReadOnlyCollection<DriverCapability>? Capacidades) : IRequest<Guid>;

public class CreateDriverCommandValidator : AbstractValidator<CreateDriverCommand>
{
    public CreateDriverCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ComissaoPercentual).InclusiveBetween(0, 100);
    }
}

public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateDriverCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = Driver.Criar(request.Nome, request.Tipo, request.ComissaoPercentual, request.Telefone,
            request.Referencia, request.Indicacao, request.FonteIndicacao);

        if (request.Capacidades is { Count: > 0 })
        {
            driver.DefinirCapacidades(request.Capacidades);
        }

        _db.Drivers.Add(driver);
        await _db.SaveChangesAsync(cancellationToken);

        return driver.Id;
    }
}
