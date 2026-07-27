namespace FleetExecutive.Domain.Common;

/// <summary>
/// Formato-padrão do log de auditoria transversal (Estrutura/06-modelo-de-dados.md — seção
/// "Auditoria — padrão transversal"). Cada entidade auditável (Pedido, Orçamento, Comissão) tem
/// sua própria tabela física de log reaproveitando este formato, em vez de uma tabela polimórfica
/// única — mantém índices e queries simples por entidade.
/// </summary>
public abstract class DomainEvent
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public Guid? AutorId { get; protected set; }
    public string TipoEvento { get; protected set; } = default!;
    public string Descricao { get; protected set; } = default!;
    public string? CampoAlterado { get; protected set; }
    public string? ValorAnterior { get; protected set; }
    public string? ValorNovo { get; protected set; }
    public string? Justificativa { get; protected set; }
    public DateTimeOffset CriadoEm { get; protected set; } = DateTimeOffset.UtcNow;

    protected DomainEvent() { }

    protected DomainEvent(Guid? autorId, string tipoEvento, string descricao, string? campoAlterado = null,
        string? valorAnterior = null, string? valorNovo = null, string? justificativa = null)
    {
        AutorId = autorId;
        TipoEvento = tipoEvento;
        Descricao = descricao;
        CampoAlterado = campoAlterado;
        ValorAnterior = valorAnterior;
        ValorNovo = valorNovo;
        Justificativa = justificativa;
    }
}
