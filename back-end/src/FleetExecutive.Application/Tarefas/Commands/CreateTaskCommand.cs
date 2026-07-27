using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Tarefas;
using FluentValidation;
using MediatR;

namespace FleetExecutive.Application.Tarefas.Commands;

public record CreateTaskCommand(
    string Titulo, PrioridadeTarefa Prioridade, Guid? ResponsavelId, string? Descricao, DateTimeOffset? Prazo,
    VinculoTarefa? VinculoTipo, Guid? VinculoId) : IRequest<Guid>;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(300);
        RuleFor(x => x.VinculoId).NotEmpty().When(x => x.VinculoTipo is not null)
            .WithMessage("VinculoId é obrigatório quando VinculoTipo é informado.");
    }
}

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateTaskCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = TaskItem.Criar(request.Titulo, request.Prioridade, request.ResponsavelId, request.Descricao,
            request.Prazo, request.VinculoTipo, request.VinculoId);

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}
