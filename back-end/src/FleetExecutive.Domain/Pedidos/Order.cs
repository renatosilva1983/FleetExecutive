using FleetExecutive.Domain.Common;
using FleetExecutive.Domain.Orcamentos;

namespace FleetExecutive.Domain.Pedidos;

/// <summary>
/// Pedido — reserva confirmada, pode ter N serviços (Estrutura/02-dominio-e-glossario.md). Gerado
/// a partir de um Orçamento (Funil → "Reserva Confirmada") ou criado direto pelo painel. Contrato
/// usa código de aceite simples (sem assinatura digital complexa na v1/v2, ver
/// Estrutura/03-sistema-atual-analise.md achado #11).
/// </summary>
public class Order : Entity
{
    public Guid? QuoteId { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid? AtendenteId { get; private set; }
    public OrigemOrcamento Origem { get; private set; }
    public StatusComercial StatusComercial { get; private set; } = StatusComercial.EmAndamento;
    public StatusOperacional StatusOperacional { get; private set; } = StatusOperacional.Confirmado;
    public string? FormaPagamento { get; private set; }
    public string? CodigoAceiteTermos { get; private set; }
    public DateTimeOffset? AceiteEm { get; private set; }
    public decimal ValorTotal { get; private set; }
    public string? MotivoCancelamento { get; private set; }

    private readonly List<OrderItem> _itens = [];
    public IReadOnlyCollection<OrderItem> Itens => _itens.AsReadOnly();

    private Order() { }

    public static Order Criar(Guid customerId, OrigemOrcamento origem, Guid? atendenteId = null, Guid? quoteId = null)
    {
        return new Order
        {
            CustomerId = customerId,
            Origem = origem,
            AtendenteId = atendenteId,
            QuoteId = quoteId,
        };
    }

    public void AdicionarItem(OrderItem item)
    {
        _itens.Add(item);
        RecalcularValorTotal();
    }

    public void RecalcularValorTotal() => ValorTotal = _itens.Sum(i => i.Subtotal);

    public void MarcarOrcamentoEnviado() => AvancarCiclo(StatusOperacional.OrcamentoEnviado);

    public void RegistrarAceiteTermos(string codigoAceite)
    {
        CodigoAceiteTermos = codigoAceite;
        AceiteEm = DateTimeOffset.UtcNow;
        AvancarCiclo(StatusOperacional.AceiteCliente);
    }

    public void AvancarCiclo(StatusOperacional novoStatus)
    {
        if (StatusOperacional == Pedidos.StatusOperacional.Finalizado && novoStatus != Pedidos.StatusOperacional.Avaliacao)
            throw new InvalidOperationException("Pedido finalizado só pode avançar para Avaliação.");

        StatusOperacional = novoStatus;
        if (novoStatus is Pedidos.StatusOperacional.Finalizado or Pedidos.StatusOperacional.Avaliacao)
        {
            StatusComercial = StatusComercial.AFaturar;
        }
    }

    public void Cancelar(string motivo)
    {
        StatusComercial = StatusComercial.Cancelado;
        MotivoCancelamento = motivo;
    }

    public void DefinirFormaPagamento(string formaPagamento) => FormaPagamento = formaPagamento.Trim();

    public void MarcarConcluido() => StatusComercial = StatusComercial.Concluido;
}
