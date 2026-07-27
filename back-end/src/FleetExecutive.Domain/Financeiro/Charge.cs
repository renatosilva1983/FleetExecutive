using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Financeiro;

/// <summary>Cobrança individual gerada a partir de um pedido (Estrutura/06-modelo-de-dados.md — charges).</summary>
public class Charge : Entity
{
    public Guid OrderId { get; private set; }
    public TipoCobranca Tipo { get; private set; }
    public StatusCobranca Status { get; private set; } = StatusCobranca.AVencer;
    public OrigemCobranca Origem { get; private set; }
    public decimal Valor { get; private set; }
    public DateOnly VenceEm { get; private set; }
    public DateTimeOffset? PagoEm { get; private set; }
    public decimal ImpostoRetido { get; private set; }

    private Charge() { }

    public static Charge Criar(Guid orderId, TipoCobranca tipo, decimal valor, DateOnly venceEm,
        OrigemCobranca origem = OrigemCobranca.Manual, decimal impostoRetido = 0)
    {
        if (valor <= 0)
            throw new ArgumentOutOfRangeException(nameof(valor), "Valor da cobrança precisa ser maior que zero.");

        return new Charge
        {
            OrderId = orderId,
            Tipo = tipo,
            Valor = valor,
            VenceEm = venceEm,
            Origem = origem,
            ImpostoRetido = impostoRetido,
        };
    }

    public void RegistrarRecebimento(bool recebidoPelaEmpresa = false)
    {
        Status = recebidoPelaEmpresa ? StatusCobranca.RecebidaEmpresa : StatusCobranca.Recebida;
        PagoEm = DateTimeOffset.UtcNow;
    }

    public void MarcarVencida() => Status = StatusCobranca.Vencida;
}
