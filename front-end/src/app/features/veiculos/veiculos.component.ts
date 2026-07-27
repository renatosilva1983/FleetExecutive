import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { VehiclesService } from '../../core/api/vehicles.service';
import { VehicleListItem } from '../../core/api/api.models';
import { veiculoBadge } from '../../shared/ui/status-badge';

@Component({
  selector: 'app-veiculos',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './veiculos.component.html',
})
export class VeiculosComponent {
  private readonly vehiclesService = inject(VehiclesService);

  protected readonly loading = signal(true);
  protected readonly error = signal(false);
  protected readonly veiculos = signal<VehicleListItem[]>([]);
  protected readonly total = signal(0);
  protected readonly busca = signal('');

  protected readonly badge = veiculoBadge;

  constructor() {
    this.load();
  }

  protected load(): void {
    this.loading.set(true);
    this.error.set(false);
    this.vehiclesService.list({ busca: this.busca().trim() || undefined, pagina: 1, tamanhoPagina: 20 }).subscribe({
      next: (page) => {
        this.veiculos.set(page.items);
        this.total.set(page.totalRegistros);
        this.loading.set(false);
      },
      error: () => {
        this.error.set(true);
        this.loading.set(false);
      },
    });
  }

  protected get disponiveis(): number {
    return this.veiculos().filter((v) => v.status === 'Disponivel').length;
  }
}
