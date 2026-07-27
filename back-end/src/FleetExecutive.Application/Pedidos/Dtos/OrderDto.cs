namespace FleetExecutive.Application.Pedidos.Dtos;

public record OrderItemDto(
    Guid Id, Guid? DriverId, Guid? VehicleId, string Tipo, string Origem, string Destino,
    DateTimeOffset DataHoraIda, DateTimeOffset? DataHoraVolta, decimal ValorServico, decimal Acrescimo,
    decimal Subtotal, string ChaveAcessoCheckin, DateTimeOffset? CheckinEm, DateTimeOffset? InicioServicoEm,
    DateTimeOffset? FimServicoEm);

public record OrderAuditLogDto(
    Guid Id, Guid? AutorId, string TipoEvento, string Descricao, string? Justificativa, DateTimeOffset CriadoEm);

public record OrderDto(
    Guid Id, Guid? QuoteId, Guid CustomerId, Guid? AtendenteId, string Origem, string StatusComercial,
    string StatusOperacional, string? FormaPagamento, string? CodigoAceiteTermos, DateTimeOffset? AceiteEm,
    decimal ValorTotal, string? MotivoCancelamento, DateTimeOffset CreatedAt,
    IReadOnlyCollection<OrderItemDto> Itens, IReadOnlyCollection<OrderAuditLogDto> Historico);

public record OrderListItemDto(
    Guid Id, Guid CustomerId, string Origem, string StatusComercial, string StatusOperacional,
    decimal ValorTotal, DateTimeOffset CreatedAt);
