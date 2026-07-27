import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OrdersService } from '../../../core/api/orders.service';
import { OrderListItem } from '../../../core/api/api.models';
import { comercialBadge } from '../../../shared/ui/status-badge';

@Component({
  selector: 'app-pedidos-list',
  imports: [RouterLink, CurrencyPipe, DatePipe],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './pedidos-list.component.html',
  styleUrl: './pedidos-list.component.scss',
})
export class PedidosListComponent {
  private readonly ordersService = inject(OrdersService);

  protected readonly loading = signal(true);
  protected readonly error = signal(false);
  protected readonly pedidos = signal<OrderListItem[]>([]);
  protected readonly total = signal(0);
  protected readonly termo = signal('');

  protected readonly badge = comercialBadge;

  constructor() {
    this.load();
  }

  protected load(): void {
    this.loading.set(true);
    this.error.set(false);
    this.ordersService.list({ pagina: 1, tamanhoPagina: 20 }).subscribe({
      next: (page) => {
        this.pedidos.set(page.items);
        this.total.set(page.totalRegistros);
        this.loading.set(false);
      },
      error: () => {
        this.error.set(true);
        this.loading.set(false);
      },
    });
  }

  protected get faturamentoTotal(): number {
    return this.pedidos().reduce((acc, p) => acc + p.valorTotal, 0);
  }

  protected get ticketMedio(): number {
    const list = this.pedidos();
    return list.length ? this.faturamentoTotal / list.length : 0;
  }

  protected filtrados(): OrderListItem[] {
    const t = this.termo().trim().toLowerCase();
    if (!t) {
      return this.pedidos();
    }
    return this.pedidos().filter(
      (p) => p.origem.toLowerCase().includes(t) || p.id.toLowerCase().includes(t),
    );
  }
}
