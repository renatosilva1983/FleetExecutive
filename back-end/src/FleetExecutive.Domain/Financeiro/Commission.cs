using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Financeiro;

/// <summary>
/// Comissão sobre um serviço (order_item), para um Atendente ou Motorista (Estrutura/06-modelo-de-dados.md).
/// Toda alteração manual de valor exige justificativa (Estrutura/03-sistema-atual-analise.md achado #3
/// — "alterada?/justificativa" observado em produção). Gerada automaticamente quando um serviço é
/// adicionado a um pedido (ver AddOrderItemCommandHandler, Estrutura/12-modulos-financeiro-e-logging.md).
/// </summary>
public class Commission : Entity
{
    public Guid OrderItemId { get; private set; }
    public TipoComissao Tipo { get; private set; }
    public TipoRecebedorComissao RecebedorTipo { get; private set; }
    public Guid RecebedorId { get; private set; }
    public decimal Percentual { get; private set; }
    public decimal Valor { get; private set; }
    public StatusComissao Status { get; private set; } = StatusComissao.Pendente;
    public DateTimeOffset? PagoEm { get; private set; }
    public bool Alterada { get; private set; }
    public string? JustificativaAlteracao { get; private set; }

    private Commission() { }

    public static Commission Criar(Guid orderItemId, TipoRecebedorComissao recebedorTipo, Guid recebedorId,
        decimal percentual, decimal valorServico, TipoComissao tipo = TipoComissao.Normal)
    {
        if (percentual is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(percentual), "Percentual precisa estar entre 0 e 100.");

        return new Commission
        {
            OrderItemId = orderItemId,
            Tipo = tipo,
            RecebedorTipo = recebedorTipo,
            RecebedorId = recebedorId,
            Percentual = percentual,
            Valor = Math.Round(valorServico * percentual / 100m, 2),
        };
    }

    /// <summary>Ajuste manual — sempre exige justificativa, marca a comissão como alterada (auditoria).</summary>
    public void AjustarValor(decimal novoValor, string justificativa)
    {
        if (string.IsNullOrWhiteSpace(justificativa))
            throw new InvalidOperationException("Justificativa é obrigatória para alterar manualmente uma comissão.");
        if (Status == StatusComissao.Paga)
            throw new InvalidOperationException("Não é possível alterar uma comissão já paga.");

        Valor = novoValor;
        Alterada = true;
        JustificativaAlteracao = justificativa;
    }

    public void MarcarPaga()
    {
        Status = StatusComissao.Paga;
        PagoEm = DateTimeOffset.UtcNow;
    }
}
