import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CustomersService } from '../../core/api/customers.service';
import { CustomerListItem } from '../../core/api/api.models';

@Component({
  selector: 'app-clientes',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './clientes.component.html',
  styleUrl: './clientes.component.scss',
})
export class ClientesComponent {
  private readonly customersService = inject(CustomersService);

  protected readonly loading = signal(true);
  protected readonly error = signal(false);
  protected readonly clientes = signal<CustomerListItem[]>([]);
  protected readonly total = signal(0);
  protected readonly busca = signal('');

  constructor() {
    this.load();
  }

  protected load(): void {
    this.loading.set(true);
    this.error.set(false);
    this.customersService.list({ busca: this.busca().trim() || undefined, pagina: 1, tamanhoPagina: 20 }).subscribe({
      next: (page) => {
        this.clientes.set(page.items);
        this.total.set(page.totalRegistros);
        this.loading.set(false);
      },
      error: () => {
        this.error.set(true);
        this.loading.set(false);
      },
    });
  }

  protected get ativos(): number {
    return this.clientes().filter((c) => c.ativo).length;
  }

  protected tipoLabel(tipo: string): string {
    if (tipo === 'PessoaJuridica') return 'PJ';
    if (tipo === 'PessoaFisica') return 'PF';
    return tipo;
  }
}
