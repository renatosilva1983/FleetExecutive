namespace FleetExecutive.Application.Clientes.Dtos;

public record CustomerDto(
    Guid Id,
    string Tipo,
    string Nome,
    string? CpfCnpj,
    string Email,
    string? Telefone,
    Guid? AtendenteId,
    bool Ativo,
    string? Observacoes,
    IReadOnlyCollection<string> Tags,
    DateTimeOffset CreatedAt);

public record CustomerListItemDto(
    Guid Id,
    string Nome,
    string Tipo,
    bool Ativo,
    string Email,
    Guid? AtendenteId);
