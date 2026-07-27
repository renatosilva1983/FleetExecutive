import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FinanceiroService } from '../../core/api/financeiro.service';
import { Charge, Commission, Invoice } from '../../core/api/api.models';
import { NotificationService } from '../../core/notifications/notification.service';
import { financeiroBadge } from '../../shared/ui/status-badge';

type Aba = 'cobrancas' | 'comissoes' | 'faturas';

@Component({
  selector: 'app-financeiro',
  imports: [CurrencyPipe, DatePipe],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './financeiro.component.html',
  styleUrl: './financeiro.component.scss',
})
export class FinanceiroComponent {
  private readonly financeiro = inject(FinanceiroService);
  private readonly notify = inject(NotificationService);

  protected readonly aba = signal<Aba>('cobrancas');
  protected readonly loading = signal(false);
  protected readonly error = signal(false);

  protected readonly cobrancas = signal<Charge[]>([]);
  protected readonly comissoes = signal<Commission[]>([]);
  protected readonly faturas = signal<Invoice[]>([]);

  protected readonly badge = financeiroBadge;

  constructor() {
    this.selecionar('cobrancas');
  }

  protected selecionar(aba: Aba): void {
    this.aba.set(aba);
    this.loading.set(true);
    this.error.set(false);

    const done = <T>(target: (v: T[]) => void) => ({
      next: (page: { items: T[] }) => {
        target(page.items);
        this.loading.set(false);
      },
      error: () => {
        this.error.set(true);
        this.loading.set(false);
      },
    });

    if (aba === 'cobrancas') {
      this.financeiro.listCharges({ pagina: 1, tamanhoPagina: 20 }).subscribe(done((v) => this.cobrancas.set(v)));
    } else if (aba === 'comissoes') {
      this.financeiro.listCommissions({ pagina: 1, tamanhoPagina: 20 }).subscribe(done((v) => this.comissoes.set(v)));
    } else {
      this.financeiro.listInvoices({ pagina: 1, tamanhoPagina: 20 }).subscribe(done((v) => this.faturas.set(v)));
    }
  }

  protected registrarRecebimento(c: Charge): void {
    this.financeiro.registerPayment(c.id, true).subscribe({
      next: () => {
        this.notify.success('Recebimento registrado.');
        this.selecionar('cobrancas');
      },
    });
  }

  protected marcarComissaoPaga(c: Commission): void {
    this.financeiro.markCommissionPaid(c.id).subscribe({
      next: () => {
        this.notify.success('Comissão marcada como paga.');
        this.selecionar('comissoes');
      },
    });
  }
}
