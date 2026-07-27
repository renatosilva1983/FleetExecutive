namespace FleetExecutive.Application.Prestadores.Dtos;

public record DriverLanguageDto(Guid Id, string Idioma, string Nivel);

public record DriverDocumentDto(Guid Id, string Tipo, string? Categoria, string? Numero, DateOnly? ValidoAte, string Status);

public record DriverDto(
    Guid Id, string Nome, string Tipo, bool Ativo, string? Telefone, string? Referencia,
    decimal ComissaoPercentual, bool Indicacao, string? FonteIndicacao,
    IReadOnlyCollection<string> Capacidades,
    IReadOnlyCollection<DriverLanguageDto> Idiomas,
    IReadOnlyCollection<DriverDocumentDto> Documentos);

public record DriverListItemDto(
    Guid Id, string Nome, string Tipo, bool Ativo, string? Referencia, string? Telefone,
    decimal ComissaoPercentual, bool Indicacao);
