using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Financeiro;

/// <summary>
/// Fatura — agrega um pedido "À Faturar" (Estrutura/06-modelo-de-dados.md — invoices). Distinta de
/// Charge: Fatura é o registro de que o pedido foi faturado; Cobrança é o lançamento financeiro
/// em si (ver Estrutura/03-sistema-atual-analise.md achado #7).
/// </summary>
public class Invoice : Entity
{
    public Guid OrderId { get; private set; }
    public StatusFatura Status { get; private set; } = StatusFatura.AFaturar;
    public DateTimeOffset? GeradoEm { get; private set; }

    private Invoice() { }

    public static Invoice Criar(Guid orderId) => new() { OrderId = orderId };

    public void MarcarFaturado() => (Status, GeradoEm) = (StatusFatura.Faturado, DateTimeOffset.UtcNow);
}
