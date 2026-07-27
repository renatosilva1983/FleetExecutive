import { ChangeDetectionStrategy, Component } from '@angular/core';

/**
 * Relatórios (portado de relatorios.html). KPIs e séries usam dados
 * representativos; ligar ao RelatoriosController quando os agregados estiverem
 * disponíveis. Gráficos são desenhados em CSS puro (sem dependências externas).
 */
@Component({
  selector: 'app-relatorios',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './relatorios.component.html',
  styleUrl: './relatorios.component.scss',
})
export class RelatoriosComponent {
  protected readonly kpis = [
    { label: 'Faturamento (6 meses)', valor: 'R$ 631.441', trend: '↑ 14%', cor: '#059669' },
    { label: 'Pedidos concluídos', valor: '102', trend: '↑ 8%', cor: '#2563eb' },
    { label: 'Ticket médio', valor: 'R$ 6.190', trend: '↑ 3%', cor: '#6d28d9' },
    { label: 'Margem bruta média', valor: '47,2%', trend: '↑ 2pp', cor: '#d97706' },
  ];

  protected readonly faturamentoMensal = [
    { mes: 'Jan', valor: 78, label: 'R$ 78k' },
    { mes: 'Fev', valor: 92, label: 'R$ 92k' },
    { mes: 'Mar', valor: 105, label: 'R$ 105k' },
    { mes: 'Abr', valor: 98, label: 'R$ 98k' },
    { mes: 'Mai', valor: 121, label: 'R$ 121k' },
    { mes: 'Jun', valor: 137, label: 'R$ 137k' },
  ];

  protected readonly topClientes = [
    { nome: 'INSPER', valor: 'R$ 142.300', pct: 92 },
    { nome: 'Luminus Seguros', valor: 'R$ 98.700', pct: 64 },
    { nome: 'Sind. Trab. Refeições SP', valor: 'R$ 74.100', pct: 48 },
    { nome: 'Doremus Alimentos', valor: 'R$ 61.200', pct: 40 },
    { nome: 'Via Italia Comércio', valor: 'R$ 43.800', pct: 28 },
  ];

  protected get maxFaturamento(): number {
    return Math.max(...this.faturamentoMensal.map((m) => m.valor));
  }
}
