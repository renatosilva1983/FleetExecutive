using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Orcamentos;

/// <summary>
/// Orçamento — proposta antes de virar Pedido (Estrutura/02-dominio-e-glossario.md). O próprio
/// Status representa a coluna do Kanban do Funil de Vendas (Estrutura/10-modulos-comercial-operacional.md).
/// Sugestão de preço por IA e seleção de prestador do wizard completo ficam para quando a
/// integração de e-mail/IA existir (v3 do roadmap) — v2 é cálculo manual, ver
/// Estrutura/13-deploy-multitenant-e-roadmap.md.
/// </summary>
public class Quote : Entity
{
    public Guid CustomerId { get; private set; }
    public Guid? AtendenteId { get; private set; }
    public OrigemOrcamento Origem { get; private set; }
    public StatusFunil Status { get; private set; } = StatusFunil.PendenteAtendimento;
    public DateOnly? DataServico { get; private set; }
    public decimal ValorEstimado { get; private set; }
    public string? MotivoPerda { get; private set; }

    private readonly List<QuoteService> _servicos = [];
    public IReadOnlyCollection<QuoteService> Servicos => _servicos.AsReadOnly();

    private Quote() { }

    public static Quote Criar(Guid customerId, OrigemOrcamento origem, decimal valorEstimado,
        Guid? atendenteId = null, DateOnly? dataServico = null)
    {
        return new Quote
        {
            CustomerId = customerId,
            Origem = origem,
            ValorEstimado = valorEstimado,
            AtendenteId = atendenteId,
            DataServico = dataServico,
        };
    }

    public void AdicionarServico(QuoteService servico) => _servicos.Add(servico);

    /// <summary>Move o orçamento para uma nova coluna do funil. "Perdido" exige motivo.</summary>
    public void AvancarFunil(StatusFunil novoStatus, string? motivoPerda = null)
    {
        if (Status == StatusFunil.ReservaConfirmada)
            throw new InvalidOperationException("Orçamento já confirmado como reserva não pode mudar de estágio.");

        if (novoStatus == StatusFunil.Perdido && string.IsNullOrWhiteSpace(motivoPerda))
            throw new InvalidOperationException("Motivo é obrigatório ao marcar um orçamento como perdido.");

        Status = novoStatus;
        MotivoPerda = novoStatus == StatusFunil.Perdido ? motivoPerda : null;
        MarkUpdated(AtendenteId);
    }

    public void AtualizarValorEstimado(decimal valorEstimado) => ValorEstimado = valorEstimado;
}
