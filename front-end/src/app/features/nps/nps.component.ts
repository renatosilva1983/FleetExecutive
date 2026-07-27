import { ChangeDetectionStrategy, Component, signal } from '@angular/core';

interface Avaliacao {
  cliente: string;
  nota: number;
  texto: string;
  servico: string;
  quando: string;
}

/**
 * NPS / Avaliações (portado de nps.html). Placar e distribuição usam dados
 * representativos até o NpsController expor os agregados; a lista de avaliações
 * está pronta para consumir a API correspondente.
 */
@Component({
  selector: 'app-nps',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './nps.component.html',
  styleUrl: './nps.component.scss',
})
export class NpsComponent {
  protected readonly npsScore = 72;
  protected readonly mediaEstrelas = 4.7;
  protected readonly totalRespostas = 128;

  protected readonly distribuicao = [
    { estrelas: 5, qtd: 86, pct: 67 },
    { estrelas: 4, qtd: 28, pct: 22 },
    { estrelas: 3, qtd: 9, pct: 7 },
    { estrelas: 2, qtd: 3, pct: 2 },
    { estrelas: 1, qtd: 2, pct: 2 },
  ];

  protected readonly avaliacoes = signal<Avaliacao[]>([
    { cliente: 'INSPER', nota: 5, texto: 'Ótimo serviço, motorista pontual e educado!', servico: 'Serviço #308', quando: 'há 2 dias' },
    { cliente: 'Luminus Seguros', nota: 4, texto: 'Ônibus limpo e confortável. Recomendo.', servico: 'Serviço #301', quando: 'há 5 dias' },
    { cliente: 'Doremus Alimentos', nota: 5, texto: 'Atendimento impecável do início ao fim.', servico: 'Serviço #295', quando: 'há 1 semana' },
    { cliente: 'Via Italia Comércio', nota: 3, texto: 'Bom, mas houve atraso na saída.', servico: 'Serviço #289', quando: 'há 2 semanas' },
  ]);

  protected estrelasArray(n: number): boolean[] {
    return [1, 2, 3, 4, 5].map((i) => i <= n);
  }
}
