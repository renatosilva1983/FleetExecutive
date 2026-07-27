using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Tarefas;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Tarefas.Commands;

public record UpdateTaskStatusCommand(Guid Id, StatusTarefa NovoStatus) : IRequest;

public class UpdateTaskStatusCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
{
    public UpdateTaskStatusCommandValidator() => RuleFor(x => x.Id).NotEmpty();
}

public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateTaskStatusCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Tarefa não encontrada.");

        task.MudarStatus(request.NovoStatus);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
