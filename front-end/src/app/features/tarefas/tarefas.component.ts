import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { TasksService } from '../../core/api/tasks.service';
import { TaskItem, TaskStatus } from '../../core/api/api.models';
import { NotificationService } from '../../core/notifications/notification.service';
import { tarefaBadge } from '../../shared/ui/status-badge';

interface Coluna {
  status: TaskStatus;
  titulo: string;
}

@Component({
  selector: 'app-tarefas',
  imports: [DatePipe],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './tarefas.component.html',
  styleUrl: './tarefas.component.scss',
})
export class TarefasComponent {
  private readonly tasksService = inject(TasksService);
  private readonly notify = inject(NotificationService);

  protected readonly loading = signal(true);
  protected readonly error = signal(false);
  protected readonly tarefas = signal<TaskItem[]>([]);

  protected readonly badge = tarefaBadge;

  protected readonly colunas: Coluna[] = [
    { status: 'Aberta', titulo: 'Abertas' },
    { status: 'EmAndamento', titulo: 'Em andamento' },
    { status: 'Concluida', titulo: 'Concluídas' },
  ];

  protected readonly porStatus = computed(() => {
    const map: Record<string, TaskItem[]> = {};
    for (const c of this.colunas) {
      map[c.status] = [];
    }
    for (const t of this.tarefas()) {
      (map[t.status] ??= []).push(t);
    }
    return map;
  });

  constructor() {
    this.load();
  }

  protected load(): void {
    this.loading.set(true);
    this.error.set(false);
    this.tasksService.list({ pagina: 1, tamanhoPagina: 50 }).subscribe({
      next: (page) => {
        this.tarefas.set(page.items);
        this.loading.set(false);
      },
      error: () => {
        this.error.set(true);
        this.loading.set(false);
      },
    });
  }

  protected avancar(t: TaskItem): void {
    const proximo: TaskStatus | null =
      t.status === 'Aberta' ? 'EmAndamento' : t.status === 'EmAndamento' ? 'Concluida' : null;
    if (!proximo) {
      return;
    }
    this.tasksService.updateStatus(t.id, proximo).subscribe({
      next: () => {
        this.tarefas.update((list) => list.map((x) => (x.id === t.id ? { ...x, status: proximo } : x)));
        this.notify.success('Tarefa atualizada.');
      },
    });
  }

  protected prioridadeCss(prioridade: string): string {
    if (prioridade === 'Alta') return 'prio-alta';
    if (prioridade === 'Media') return 'prio-media';
    return 'prio-baixa';
  }
}
