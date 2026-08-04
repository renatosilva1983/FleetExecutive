using System.ComponentModel;

namespace FleetExecutive.Domain.Pedidos;

/// <summary>Status comercial/financeiro do pedido — independente do ciclo operacional (Estrutura/02-dominio-e-glossario.md).</summary>
public enum StatusComercial
{
    [Description("A Faturar")]    AFaturar = 1,
    [Description("Concluído")]    Concluido = 2,
    [Description("Em Andamento")] EmAndamento = 3,
    Cancelado = 4,
}

/// <summary>Ciclo operacional do serviço (Estrutura/02-dominio-e-glossario.md — 8 passos observados no protótipo/produção).</summary>
public enum StatusOperacional
{
    Confirmado = 1,
    [Description("Orçamento Enviado")] OrcamentoEnviado = 2,
    [Description("Aceite do Cliente")] AceiteCliente = 3,
    [Description("Check-in")]          CheckIn = 4,
    [Description("A Caminho")]         ACaminho = 5,
    [Description("Em Serviço")]        EmServico = 6,
    Finalizado = 7,
    [Description("Avaliação")]         Avaliacao = 8,
}

/// <summary>Tipos de evento do log de auditoria transversal (Estrutura/06-modelo-de-dados.md — order_audit_log).</summary>
public enum TipoEventoAuditoria
{
    Criado,
    OrcamentoEnviado,
    AceiteTermos,
    CheckinRegistrado,
    ClienteNotificado,
    InicioServico,
    FimServico,
    AlteracaoCampo,
    AlteracaoComissao,
    JustificativaAcrescimo,
    Cancelado,
}
