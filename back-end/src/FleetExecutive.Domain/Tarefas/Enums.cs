using System.ComponentModel;

namespace FleetExecutive.Domain.Tarefas;

public enum PrioridadeTarefa
{
    Alta = 1,
    [Description("Média")] Media = 2,
    Baixa = 3,
}

public enum StatusTarefa
{
    [Description("A Fazer")]      AFazer = 1,
    [Description("Em Andamento")] EmAndamento = 2,
    [Description("Concluída")]    Concluida = 3,
}

/// <summary>Estrutura/03-sistema-atual-analise.md achado #14 — tarefas reais vinculam a Orçamento, não só Pedido/Cliente.</summary>
public enum VinculoTarefa
{
    Pedido = 1,
    [Description("Orçamento")] Orcamento = 2,
    Cliente = 3,
}
