import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

interface AiOption {
  key: 'economico' | 'recomendado' | 'premium';
  label: string;
  preco: string;
  custo: string;
  margem: string;
  pct: string;
  nota: string;
  destaque?: boolean;
}

interface Provider {
  inicial: string;
  bg: string;
  nome: string;
  indicacao?: boolean;
  estrelas: number;
  meta: string;
  tags: { texto: string; cor: string }[];
  custo: string;
  preco: string;
  margem: string;
  margemPct: string;
  esmaecido?: boolean;
}

/**
 * Novo orçamento com sugestão de IA (portado de orcamento-novo.html — etapa
 * "Serviço" do wizard). Os blocos de IA/prestadores são representativos; a
 * seleção é reativa. Ligar ao QuotesService.create() quando o fluxo de
 * criação de orçamento estiver definido no backend.
 */
@Component({
  selector: 'app-orcamento-novo',
  imports: [RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './orcamento-novo.component.html',
  styleUrl: './orcamento-novo.component.scss',
})
export class OrcamentoNovoComponent {
  protected readonly bannerVisivel = signal(true);
  protected readonly aiSelecionado = signal<AiOption['key']>('recomendado');
  protected readonly providerSelecionado = signal(0);

  protected readonly steps = [
    { num: '✓', label: 'Cliente', state: 'done' },
    { num: '2', label: 'Serviço', state: 'active' },
    { num: '3', label: 'Roteiro', state: '' },
    { num: '4', label: 'Prestadores', state: '' },
    { num: '5', label: 'Valores', state: '' },
    { num: '6', label: 'Confirmação', state: '' },
  ];

  protected readonly features = [
    { icon: '🛡', titulo: 'Veículo blindado', desc: 'Proteção nível III-A ou superior', checked: false },
    { icon: '🌐', titulo: 'Profissional bilíngue', desc: 'Atendimento em outro idioma', checked: true },
    { icon: '☕', titulo: 'Serviço de bordo', desc: 'Água, café, snacks a bordo', checked: true },
    { icon: '🏷', titulo: 'Adesivação', desc: 'Identidade visual do cliente no veículo', checked: false },
  ];

  protected readonly aiOptions: AiOption[] = [
    { key: 'economico', label: 'Econômico', preco: 'R$ 3.800', custo: 'R$ 2.200', margem: 'R$ 1.600', pct: '(42%)', nota: 'Menor valor histórico' },
    { key: 'recomendado', label: '⭐ Recomendado', preco: 'R$ 4.560', custo: 'R$ 2.600', margem: 'R$ 1.960', pct: '(43%)', nota: 'Média dos últimos 6 meses', destaque: true },
    { key: 'premium', label: 'Premium', preco: 'R$ 5.200', custo: 'R$ 3.100', margem: 'R$ 2.100', pct: '(40%)', nota: 'Maior valor histórico' },
  ];

  protected readonly providers: Provider[] = [
    {
      inicial: 'E', bg: 'linear-gradient(135deg,#4c1d95,#6d28d9)', nome: 'EDSON', indicacao: true, estrelas: 5,
      meta: '4.9 · 23 serviços · Ônibus Exec. 46L',
      tags: [
        { texto: '✅ Disponível 15/08', cor: '#10b981' },
        { texto: '🌐 Bilíngue (EN)', cor: '#3b82f6' },
        { texto: '☕ Serviço de bordo', cor: '#f59e0b' },
      ],
      custo: 'R$ 2.200', preco: 'R$ 4.560', margem: 'R$ 2.360', margemPct: '51,8%',
    },
    {
      inicial: 'G', bg: 'linear-gradient(135deg,#1d4ed8,#3b82f6)', nome: 'Grecia Turismo', estrelas: 5,
      meta: '4.8 · 15 serviços · Ônibus Exec. 46L',
      tags: [
        { texto: '✅ Disponível 15/08', cor: '#10b981' },
        { texto: '☕ Serviço de bordo', cor: '#f59e0b' },
      ],
      custo: 'R$ 2.400', preco: 'R$ 4.560', margem: 'R$ 2.160', margemPct: '47,4%',
    },
    {
      inicial: 'I', bg: 'linear-gradient(135deg,#374151,#6b7280)', nome: 'Igor Martins', estrelas: 4,
      meta: '4.2 · 8 serviços · Ônibus Exec. 50L',
      tags: [{ texto: '⚠ Verificar disponibilidade', cor: '#f59e0b' }],
      custo: 'R$ 2.100', preco: 'R$ 4.560', margem: 'R$ 2.460', margemPct: '53,9%', esmaecido: true,
    },
  ];

  protected estrelasArray(n: number): boolean[] {
    return [1, 2, 3, 4, 5].map((i) => i <= n);
  }
}
