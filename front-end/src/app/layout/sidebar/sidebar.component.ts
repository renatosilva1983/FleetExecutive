import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

interface NavItem {
  label: string;
  icon: string;
  route: string;
  /** Rota-índice: só fica ativa em correspondência exata (evita casar tudo com '/'). */
  exact?: boolean;
  badge?: string;
  isNew?: boolean;
}

interface NavGroup {
  label?: string;
  sub?: boolean;
  items: NavItem[];
}

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <nav class="sidebar">
      <div class="sidebar-logo">
        <div class="logo-mark">B</div>
        <div>
          <div class="logo-text">FleetExecutive</div>
          <div class="logo-sub">Orgulho em transportar você!</div>
        </div>
        <div class="collapse-btn">‹</div>
      </div>

      @for (group of groups; track group.label) {
        @if (group.label) {
          <div class="nav-section-label">{{ group.label }}</div>
        }
        <div [class.nav-sub]="group.sub" [class.nav-main]="!group.sub">
          @for (item of group.items; track item.route) {
            <a
              class="nav-item"
              [routerLink]="item.route"
              routerLinkActive="active"
              [routerLinkActiveOptions]="{ exact: !!item.exact }"
            >
              <span class="nav-icon">{{ item.icon }}</span> {{ item.label }}
              @if (item.badge) { <span class="nav-badge">{{ item.badge }}</span> }
              @if (item.isNew) { <span class="nav-badge-new">NOVO</span> }
            </a>
          }
        </div>
      }

      <button class="suporte-btn">🎁 Suporte Fretatech</button>
    </nav>
  `,
})
export class SidebarComponent {
  protected readonly groups: NavGroup[] = [
    {
      items: [
        { label: 'Dashboard', icon: '🏠', route: '/', exact: true },
        { label: 'Novo orçamento', icon: '✎', route: '/orcamentos/novo' },
        { label: 'Funil de vendas', icon: '📊', route: '/funil', badge: '169' },
        { label: 'Agenda de veículos', icon: '📅', route: '/agenda' },
        { label: 'Monitoramento ao vivo', icon: '📹', route: '/monitoramento', isNew: true },
        { label: 'Tarefas', icon: '🗒', route: '/tarefas' },
      ],
    },
    {
      label: 'Gestão',
      sub: true,
      items: [
        { label: 'Pedidos', icon: '📋', route: '/pedidos' },
        { label: 'Prestadores', icon: '👤', route: '/prestadores', isNew: true },
        { label: 'Veículos', icon: '🚌', route: '/veiculos' },
        { label: 'Clientes', icon: '👥', route: '/clientes' },
        { label: 'Financeiro', icon: '💲', route: '/financeiro' },
        { label: 'NPS / Avaliações', icon: '⭐', route: '/nps', isNew: true },
        { label: 'Relatórios', icon: '📈', route: '/relatorios' },
      ],
    },
    {
      label: 'Configurações',
      sub: true,
      items: [
        { label: 'Integração E-mail', icon: '💌', route: '/email-integracao', isNew: true },
        { label: 'Empresa', icon: '⚙', route: '/empresa' },
      ],
    },
  ];
}
