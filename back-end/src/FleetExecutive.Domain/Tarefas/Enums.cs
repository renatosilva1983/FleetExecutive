namespace FleetExecutive.Domain.Tarefas;

public enum PrioridadeTarefa { Alta = 1, Media = 2, Baixa = 3 }

public enum StatusTarefa { AFazer = 1, EmAndamento = 2, Concluida = 3 }

/// <summary>Estrutura/03-sistema-atual-analise.md achado #14 — tarefas reais vinculam a Orçamento, não só Pedido/Cliente.</summary>
public enum VinculoTarefa { Pedido = 1, Orcamento = 2, Cliente = 3 }
