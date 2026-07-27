import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';

/**
 * Dashboard — visão geral (portado de index.html). Os blocos (estatísticas do
 * mês, caixa de entrada de orçamentos, serviços em andamento, pendências e
 * avaliações) hoje usam dados representativos expostos como estado do
 * componente; cada bloco está pronto para ser ligado ao seu serviço/endpoint.
 */
@Component({
  selector: 'app-dashboard',
  imports: [RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  protected readonly periodo = 'junho 2026 — dados do mês atual';

  protected readonly stats = [
    { icon: '🚌', bg: '#6d28d920', valor: '12', label: 'Reservas', trend: '↑ 8%' },
    { icon: '🌐', bg: '#f59e0b20', valor: '295', label: 'Pesquisas', trend: '↑ 12%' },
    { icon: '👥', bg: '#ec489920', valor: '257', label: 'Clientes Novos', trend: '↑ 5%' },
    { icon: '📄', bg: '#3b82f620', valor: '233', label: 'Orçamentos', trend: '↑ 21%' },
    { icon: '$', bg: '#10b98120', valor: 'R$ 68.813', label: 'Em Pedidos', trend: '↑ 3%', small: true },
  ];

  protected readonly inbox = [
    {
      inicial: 'I', bg: 'linear-gradient(135deg,#1d4ed8,#3b82f6)',
      de: 'INSPER — insper@email.com.br', tempo: 'há 18 min',
      assunto: 'Solicitação de orçamento — transporte 45 pessoas — SP x Campos do Jordão — 15/08/2026',
      vincular: true,
    },
    {
      inicial: 'L', bg: 'linear-gradient(135deg,#065f46,#10b981)',
      de: 'Luminus Seguros — financeiro@luminus.com.br', tempo: 'há 1h',
      assunto: 'Frete executivo — 8 pessoas — São Paulo x Rio de Janeiro — 22/07/2026',
      vincular: false,
    },
  ];

  protected readonly servicos = [
    {
      id: 'Serviço #319 — Pedido #165', cliente: 'Sind. Trab. Refeições SP',
      rota: 'São Paulo → Praia Grande', passo: 2,
      status: '📍 Em deslocamento para embarque', motorista: 'EDSON · Ônibus Exec. 46L', online: true,
    },
    {
      id: 'Serviço #310 — Pedido #162', cliente: 'INSPER',
      rota: 'São Paulo → Campos do Jordão', passo: 3,
      status: '🚌 Em serviço', motorista: 'Igor Martins · Ônibus Exec. 50L', online: true,
    },
    {
      id: 'Serviço #285 — Pedido #158', cliente: 'Doremus Alimentos Ltda',
      rota: 'São Paulo → Santos', passo: 1,
      status: '⏳ Aguardando check-in do prestador',
      motorista: '⚠ Prestador ainda não registrou chegada', online: false, alerta: true,
    },
  ];

  protected readonly pendencias = [
    { id: '156', cliente: 'Sind. Trab. Refeições SP', valor: 'R$ 3.694' },
    { id: '157', cliente: 'Sind. Trab. Refeições SP', valor: 'R$ 3.694' },
  ];

  protected readonly avaliacoes = [
    { cliente: 'INSPER', nota: 5, texto: 'Ótimo serviço, motorista pontual e educado!', ref: 'Serviço #308 · há 2 dias' },
    { cliente: 'Luminus Seguros', nota: 4, texto: 'Ônibus limpo e confortável. Recomendo.', ref: 'Serviço #301 · há 5 dias' },
  ];

  protected pips(n: number): Array<'done' | 'active' | ''> {
    return [0, 1, 2, 3, 4].map((i) => (i < n ? 'done' : i === n ? 'active' : ''));
  }

  protected stars(n: number): boolean[] {
    return [1, 2, 3, 4, 5].map((i) => i <= n);
  }
}
