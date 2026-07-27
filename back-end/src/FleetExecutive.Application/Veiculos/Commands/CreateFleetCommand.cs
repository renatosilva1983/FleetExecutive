using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Veiculos;
using FluentValidation;
using MediatR;

namespace FleetExecutive.Application.Veiculos.Commands;

public record CreateFleetCommand(
    string Titulo, TipoVeiculo Tipo, CategoriaVeiculo Categoria, int Capacidade, string? Descricao,
    bool TemWc, bool TemAr, bool TemWifi, bool TemAntt, Guid? GaragemId) : IRequest<Guid>;

public class CreateFleetCommandValidator : AbstractValidator<CreateFleetCommand>
{
    public CreateFleetCommandValidator()
    {
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Capacidade).GreaterThan(0);
    }
}

public class CreateFleetCommandHandler : IRequestHandler<CreateFleetCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateFleetCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateFleetCommand request, CancellationToken cancellationToken)
    {
        var fleet = Fleet.Criar(request.Titulo, request.Tipo, request.Categoria, request.Capacidade,
            request.Descricao, request.TemWc, request.TemAr, request.TemWifi, request.TemAntt, request.GaragemId);

        _db.Fleets.Add(fleet);
        await _db.SaveChangesAsync(cancellationToken);

        return fleet.Id;
    }
}
