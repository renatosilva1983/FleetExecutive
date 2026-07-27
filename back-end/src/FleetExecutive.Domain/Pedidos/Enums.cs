namespace FleetExecutive.Domain.Pedidos;

/// <summary>Status comercial/financeiro do pedido — independente do ciclo operacional (Estrutura/02-dominio-e-glossario.md).</summary>
public enum StatusComercial
{
    AFaturar = 1,
    Concluido = 2,
    EmAndamento = 3,
    Cancelado = 4,
}

/// <summary>Ciclo operacional do serviço (Estrutura/02-dominio-e-glossario.md — 8 passos observados no protótipo/produção).</summary>
public enum StatusOperacional
{
    Confirmado = 1,
    OrcamentoEnviado = 2,
    AceiteCliente = 3,
    CheckIn = 4,
    ACaminho = 5,
    EmServico = 6,
    Finalizado = 7,
    Avaliacao = 8,
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
