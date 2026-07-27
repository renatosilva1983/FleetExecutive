import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AgendaService } from '../../core/api/agenda.service';
import { VehicleAvailability } from '../../core/api/api.models';

@Component({
  selector: 'app-agenda',
  imports: [DatePipe, FormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './agenda.component.html',
  styleUrl: './agenda.component.scss',
})
export class AgendaComponent {
  private readonly agenda = inject(AgendaService);

  protected readonly loading = signal(false);
  protected readonly error = signal(false);
  protected readonly veiculos = signal<VehicleAvailability[]>([]);

  protected dataInicio = this.hoje();
  protected dataFim = this.emDias(7);

  constructor() {
    this.load();
  }

  protected load(): void {
    this.loading.set(true);
    this.error.set(false);
    this.agenda.disponibilidade({ dataInicio: this.dataInicio, dataFim: this.dataFim }).subscribe({
      next: (list) => {
        this.veiculos.set(list);
        this.loading.set(false);
      },
      error: () => {
        this.error.set(true);
        this.loading.set(false);
      },
    });
  }

  private hoje(): string {
    return new Date().toISOString().slice(0, 10);
  }

  private emDias(dias: number): string {
    const d = new Date();
    d.setDate(d.getDate() + dias);
    return d.toISOString().slice(0, 10);
  }
}
