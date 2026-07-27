using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Tarefas;

/// <summary>
/// Nomeada "TaskItem" (não "Task") para não colidir com System.Threading.Tasks.Task.
/// Estrutura/06-modelo-de-dados.md — tasks. Vínculo opcional com Pedido/Orçamento/Cliente
/// (Estrutura/03-sistema-atual-analise.md achado #14 — estende o protótipo, que só vinculava a
/// Pedido/Cliente).
/// </summary>
public class TaskItem : Entity
{
    public string Titulo { get; private set; } = default!;
    public string? Descricao { get; private set; }
    public Guid? ResponsavelId { get; private set; }
    public PrioridadeTarefa Prioridade { get; private set; }
    public StatusTarefa Status { get; private set; } = StatusTarefa.AFazer;
    public DateTimeOffset? Prazo { get; private set; }
    public VinculoTarefa? VinculoTipo { get; private set; }
    public Guid? VinculoId { get; private set; }

    private TaskItem() { }

    public static TaskItem Criar(string titulo, PrioridadeTarefa prioridade, Guid? responsavelId = null,
        string? descricao = null, DateTimeOffset? prazo = null, VinculoTarefa? vinculoTipo = null,
        Guid? vinculoId = null)
    {
        if (vinculoTipo is not null && vinculoId is null)
            throw new InvalidOperationException("VinculoId é obrigatório quando VinculoTipo é informado.");

        return new TaskItem
        {
            Titulo = titulo.Trim(),
            Descricao = descricao?.Trim(),
            ResponsavelId = responsavelId,
            Prioridade = prioridade,
            Prazo = prazo,
            VinculoTipo = vinculoTipo,
            VinculoId = vinculoId,
        };
    }

    public void Atualizar(string titulo, string? descricao, Guid? responsavelId, PrioridadeTarefa prioridade,
        DateTimeOffset? prazo)
    {
        Titulo = titulo.Trim();
        Descricao = descricao?.Trim();
        ResponsavelId = responsavelId;
        Prioridade = prioridade;
        Prazo = prazo;
        MarkUpdated(responsavelId);
    }

    public void MudarStatus(StatusTarefa novoStatus) => Status = novoStatus;
}
