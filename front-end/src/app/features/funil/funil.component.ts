import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { QuotesService } from '../../core/api/quotes.service';
import { QuoteListItem } from '../../core/api/api.models';

interface Estagio {
  status: string;
  titulo: string;
}

@Component({
  selector: 'app-funil',
  imports: [CurrencyPipe, DatePipe, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './funil.component.html',
  styleUrl: './funil.component.scss',
})
export class FunilComponent {
  private readonly quotesService = inject(QuotesService);

  protected readonly loading = signal(true);
  protected readonly error = signal(false);
  protected readonly orcamentos = signal<QuoteListItem[]>([]);

  /** Estágios do funil (rótulos amigáveis). O `status` casa com o enum do backend. */
  protected readonly estagios: Estagio[] = [
    { status: 'Novo', titulo: '🆕 Novos' },
    { status: 'Enviado', titulo: '📤 Enviados' },
    { status: 'EmNegociacao', titulo: '💬 Em negociação' },
    { status: 'Ganho', titulo: '✅ Ganhos' },
    { status: 'Perdido', titulo: '❌ Perdidos' },
  ];

  protected readonly porEstagio = computed(() => {
    const map: Record<string, QuoteListItem[]> = {};
    for (const e of this.estagios) {
      map[e.status] = [];
    }
    for (const q of this.orcamentos()) {
      (map[q.status] ??= []).push(q);
    }
    return map;
  });

  constructor() {
    this.load();
  }

  protected load(): void {
    this.loading.set(true);
    this.error.set(false);
    this.quotesService.list({ pagina: 1, tamanhoPagina: 100 }).subscribe({
      next: (page) => {
        this.orcamentos.set(page.items);
        this.loading.set(false);
      },
      error: () => {
        this.error.set(true);
        this.loading.set(false);
      },
    });
  }

  protected total(status: string): number {
    return (this.porEstagio()[status] ?? []).reduce((acc, q) => acc + q.valorEstimado, 0);
  }
}
