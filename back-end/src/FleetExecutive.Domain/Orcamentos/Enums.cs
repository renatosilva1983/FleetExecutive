using System.ComponentModel;

namespace FleetExecutive.Domain.Orcamentos;

/// <summary>Estágios do funil de vendas (Estrutura/02-dominio-e-glossario.md) — o próprio status do Orçamento representa a coluna do Kanban.</summary>
public enum StatusFunil
{
    [Description("Oportunidade (E-mail)")] OportunidadeEmail = 1,
    [Description("Pendente Atendimento")]  PendenteAtendimento = 2,
    [Description("Contato Feito")]         ContatoFeito = 3,
    [Description("Orçamento Enviado")]     OrcamentoEnviado = 4,
    [Description("Negociação")]            Negociacao = 5,
    [Description("Reserva Confirmada")]    ReservaConfirmada = 6,
    Perdido = 7,
}

/// <summary>Origens reais observadas em produção (Estrutura/03-sistema-atual-analise.md) — "CGD" é uma origem real vista no sistema.</summary>
public enum OrigemOrcamento
{
    Painel = 1,
    [Description("E-mail IA")] EmailIA = 2,
    Online = 3,
    [Description("CGD")]       Cgd = 4,
}

public enum TipoServico
{
    [Description("Transporte Executivo")] TransporteExecutivo = 1,
    Receptivo = 2,
    [Description("Guia de Turismo")]      GuiaTurismo = 3,
    [Description("Percurso Turístico")]   PercursoTuristico = 4,
}

public enum SubtipoServico
{
    [Description("Ida e Volta")]         IdaVolta = 1,
    [Description("Só Ida")]              SoIda = 2,
    Transfer = 3,
    [Description("Fretamento Contínuo")] FretamentoContinuo = 4,
}

/// <summary>Características combináveis do serviço (Estrutura/04-prototipo-mapeamento-telas.md).</summary>
public enum CaracteristicaServico
{
    Blindado = 1,
    Bilingue = 2,
    ServicoBordo = 3,
    Adesivacao = 4,
}
