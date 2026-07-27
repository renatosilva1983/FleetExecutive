using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Tarefas;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Tarefas.Commands;

public record UpdateTaskCommand(
    Guid Id, string Titulo, string? Descricao, Guid? ResponsavelId, PrioridadeTarefa Prioridade,
    DateTimeOffset? Prazo) : IRequest;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(300);
    }
}

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateTaskCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Tarefa não encontrada.");

        task.Atualizar(request.Titulo, request.Descricao, request.ResponsavelId, request.Prioridade, request.Prazo);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
