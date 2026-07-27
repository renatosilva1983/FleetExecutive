import { ChangeDetectionStrategy, Component, inject, input, OnInit, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OrdersService } from '../../../core/api/orders.service';
import { Order } from '../../../core/api/api.models';
import { comercialBadge } from '../../../shared/ui/status-badge';

@Component({
  selector: 'app-pedido-detalhe',
  imports: [CurrencyPipe, DatePipe, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './pedido-detalhe.component.html',
  styleUrl: './pedido-detalhe.component.scss',
})
export class PedidoDetalheComponent implements OnInit {
  private readonly ordersService = inject(OrdersService);

  /** Vem da rota `pedidos/:id` via withComponentInputBinding(). */
  readonly id = input.required<string>();

  protected readonly loading = signal(true);
  protected readonly error = signal(false);
  protected readonly pedido = signal<Order | null>(null);

  protected readonly badge = comercialBadge;

  ngOnInit(): void {
    // Component input binding garante que `id` já está resolvido em ngOnInit.
    this.load();
  }

  protected load(): void {
    this.loading.set(true);
    this.error.set(false);
    this.ordersService.getById(this.id()).subscribe({
      next: (order) => {
        this.pedido.set(order);
        this.loading.set(false);
      },
      error: () => {
        this.error.set(true);
        this.loading.set(false);
      },
    });
  }
}
