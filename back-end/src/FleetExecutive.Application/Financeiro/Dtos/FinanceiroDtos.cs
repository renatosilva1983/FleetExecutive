namespace FleetExecutive.Application.Financeiro.Dtos;

public record CommissionDto(
    Guid Id, Guid OrderItemId, string Tipo, string RecebedorTipo, Guid RecebedorId, decimal Percentual,
    decimal Valor, string Status, DateTimeOffset? PagoEm, bool Alterada, string? JustificativaAlteracao);

public record ChargeDto(
    Guid Id, Guid OrderId, string Tipo, string Status, string Origem, decimal Valor, DateOnly VenceEm,
    DateTimeOffset? PagoEm, decimal ImpostoRetido);

public record InvoiceDto(Guid Id, Guid OrderId, string Status, DateTimeOffset? GeradoEm);
