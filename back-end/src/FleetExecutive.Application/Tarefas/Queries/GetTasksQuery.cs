using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Common.Models;
using FleetExecutive.Application.Tarefas.Dtos;
using FleetExecutive.Domain.Tarefas;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Tarefas.Queries;

/// <summary>Base do Kanban de Tarefas (A Fazer/Em Andamento/Concluídas) — mesma listagem, agrupada por Status no frontend.</summary>
public record GetTasksQuery(
    StatusTarefa? Status, Guid? ResponsavelId, VinculoTarefa? VinculoTipo, Guid? VinculoId,
    int Pagina = 1, int TamanhoPagina = 50) : IRequest<PaginatedList<TaskItemDto>>;

public class GetTasksQueryValidator : AbstractValidator<GetTasksQuery>
{
    public GetTasksQueryValidator()
    {
        RuleFor(x => x.Pagina).GreaterThan(0);
        RuleFor(x => x.TamanhoPagina).InclusiveBetween(1, 200);
    }
}

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, PaginatedList<TaskItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetTasksQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public Task<PaginatedList<TaskItemDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Tasks.AsNoTracking().AsQueryable();

        if (request.Status is not null)
        {
            query = query.Where(t => t.Status == request.Status);
        }

        if (request.ResponsavelId is not null)
        {
            query = query.Where(t => t.ResponsavelId == request.ResponsavelId);
        }

        if (request.VinculoTipo is not null)
        {
            query = query.Where(t => t.VinculoTipo == request.VinculoTipo);
        }

        if (request.VinculoId is not null)
        {
            query = query.Where(t => t.VinculoId == request.VinculoId);
        }

        var projected = query
            .OrderBy(t => t.Prazo)
            .Select(t => new TaskItemDto(t.Id, t.Titulo, t.Descricao, t.ResponsavelId, t.Prioridade.ToString(),
                t.Status.ToString(), t.Prazo, t.VinculoTipo != null ? t.VinculoTipo.ToString() : null, t.VinculoId,
                t.CreatedAt));

        return PaginatedList<TaskItemDto>.CreateAsync(projected, request.Pagina, request.TamanhoPagina, cancellationToken);
    }
}
