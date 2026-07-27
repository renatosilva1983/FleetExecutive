import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { DriversService } from '../../core/api/drivers.service';
import { DriverListItem } from '../../core/api/api.models';

@Component({
  selector: 'app-prestadores',
  imports: [DecimalPipe],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './prestadores.component.html',
  styleUrl: './prestadores.component.scss',
})
export class PrestadoresComponent {
  private readonly driversService = inject(DriversService);

  protected readonly loading = signal(true);
  protected readonly error = signal(false);
  protected readonly prestadores = signal<DriverListItem[]>([]);
  protected readonly total = signal(0);
  protected readonly busca = signal('');

  constructor() {
    this.load();
  }

  protected load(): void {
    this.loading.set(true);
    this.error.set(false);
    this.driversService.list({ busca: this.busca().trim() || undefined, pagina: 1, tamanhoPagina: 20 }).subscribe({
      next: (page) => {
        this.prestadores.set(page.items);
        this.total.set(page.totalRegistros);
        this.loading.set(false);
      },
      error: () => {
        this.error.set(true);
        this.loading.set(false);
      },
    });
  }

  protected get indicados(): number {
    return this.prestadores().filter((p) => p.indicacao).length;
  }
}
