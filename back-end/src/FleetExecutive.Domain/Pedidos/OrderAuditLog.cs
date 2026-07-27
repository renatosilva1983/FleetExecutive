using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Pedidos;

/// <summary>
/// Reaproveita o formato padrão de DomainEvent (Estrutura/06-modelo-de-dados.md — "Auditoria,
/// padrão transversal"). Réplica do histórico já observado em produção (Estrutura/03-sistema-atual-analise.md
/// achado #10): autor + timestamp + descrição textual por evento.
/// </summary>
public class OrderAuditLog : DomainEvent
{
    public Guid OrderId { get; private set; }

    private OrderAuditLog() { }

    private OrderAuditLog(Guid orderId, Guid? autorId, string tipoEvento, string descricao,
        string? campoAlterado, string? valorAnterior, string? valorNovo, string? justificativa)
        : base(autorId, tipoEvento, descricao, campoAlterado, valorAnterior, valorNovo, justificativa)
    {
        OrderId = orderId;
    }

    public static OrderAuditLog Criar(Guid orderId, Guid? autorId, TipoEventoAuditoria tipoEvento, string descricao,
        string? campoAlterado = null, string? valorAnterior = null, string? valorNovo = null,
        string? justificativa = null)
        => new(orderId, autorId, tipoEvento.ToString(), descricao, campoAlterado, valorAnterior, valorNovo, justificativa);
}
