namespace FleetExecutive.Application.Tarefas.Dtos;

public record TaskItemDto(
    Guid Id, string Titulo, string? Descricao, Guid? ResponsavelId, string Prioridade, string Status,
    DateTimeOffset? Prazo, string? VinculoTipo, Guid? VinculoId, DateTimeOffset CreatedAt);
