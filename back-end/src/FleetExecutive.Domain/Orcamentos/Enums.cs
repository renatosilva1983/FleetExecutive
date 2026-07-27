namespace FleetExecutive.Domain.Orcamentos;

/// <summary>Estágios do funil de vendas (Estrutura/02-dominio-e-glossario.md) — o próprio status do Orçamento representa a coluna do Kanban.</summary>
public enum StatusFunil
{
    OportunidadeEmail = 1,
    PendenteAtendimento = 2,
    ContatoFeito = 3,
    OrcamentoEnviado = 4,
    Negociacao = 5,
    ReservaConfirmada = 6,
    Perdido = 7,
}

/// <summary>Origens reais observadas em produção (Estrutura/03-sistema-atual-analise.md) — "CGD" é uma origem real vista no sistema.</summary>
public enum OrigemOrcamento
{
    Painel = 1,
    EmailIA = 2,
    Online = 3,
    Cgd = 4,
}

public enum TipoServico
{
    TransporteExecutivo = 1,
    Receptivo = 2,
    GuiaTurismo = 3,
    PercursoTuristico = 4,
}

public enum SubtipoServico
{
    IdaVolta = 1,
    SoIda = 2,
    Transfer = 3,
    FretamentoContinuo = 4,
}

/// <summary>Características combináveis do serviço (Estrutura/04-prototipo-mapeamento-telas.md).</summary>
public enum CaracteristicaServico
{
    Blindado = 1,
    Bilingue = 2,
    ServicoBordo = 3,
    Adesivacao = 4,
}
