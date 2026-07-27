namespace FleetExecutive.Application.Orcamentos.Dtos;

public record QuoteServiceDto(
    Guid Id, string TipoServico, string Subtipo, DateOnly DataIda, TimeOnly HoraIda, string Origem,
    string Destino, DateOnly? DataVolta, TimeOnly? HoraVolta, int NumPassageiros,
    string? TipoVeiculoPreferido, IReadOnlyCollection<string> Caracteristicas, string? IdiomaRequerido,
    string? Observacoes);

public record QuoteDto(
    Guid Id, Guid CustomerId, Guid? AtendenteId, string Origem, string Status, DateOnly? DataServico,
    decimal ValorEstimado, string? MotivoPerda, DateTimeOffset CreatedAt,
    IReadOnlyCollection<QuoteServiceDto> Servicos);

public record QuoteListItemDto(
    Guid Id, Guid CustomerId, string Origem, string Status, DateOnly? DataServico, decimal ValorEstimado,
    DateTimeOffset CreatedAt);
