import { Routes } from '@angular/router';
import { authGuard, guestGuard } from './core/auth/auth.guards';

/**
 * Roteamento raiz. Tudo autenticado vive sob o MainLayout (sidebar + topbar);
 * o login fica fora da casca. Todas as páginas são standalone e carregadas de
 * forma lazy (loadComponent) para manter o bundle inicial enxuto.
 *
 * `data.title` / `data.breadcrumb` alimentam a topbar.
 */
export const routes: Routes = [
  {
    path: 'login',
    canActivate: [guestGuard],
    loadComponent: () =>
      import('./features/auth/login/login.component').then((m) => m.LoginComponent),
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./layout/main-layout/main-layout.component').then((m) => m.MainLayoutComponent),
    children: [
      {
        path: '',
        pathMatch: 'full',
        data: { title: 'Dashboard', breadcrumb: 'Início' },
        loadComponent: () =>
          import('./features/dashboard/dashboard.component').then((m) => m.DashboardComponent),
      },
      {
        path: 'orcamentos/novo',
        data: { title: 'Novo orçamento', breadcrumb: 'Comercial' },
        loadComponent: () =>
          import('./features/orcamentos/orcamento-novo/orcamento-novo.component').then((m) => m.OrcamentoNovoComponent),
      },
      {
        path: 'funil',
        data: { title: 'Funil de vendas', breadcrumb: 'Comercial' },
        loadComponent: () =>
          import('./features/funil/funil.component').then((m) => m.FunilComponent),
      },
      {
        path: 'agenda',
        data: { title: 'Agenda de veículos', breadcrumb: 'Operacional' },
        loadComponent: () =>
          import('./features/agenda/agenda.component').then((m) => m.AgendaComponent),
      },
      {
        path: 'monitoramento',
        data: { title: 'Monitoramento ao vivo', breadcrumb: 'Operacional' },
        loadComponent: () =>
          import('./features/monitoramento/monitoramento.component').then((m) => m.MonitoramentoComponent),
      },
      {
        path: 'tarefas',
        data: { title: 'Tarefas', breadcrumb: 'Operacional' },
        loadComponent: () =>
          import('./features/tarefas/tarefas.component').then((m) => m.TarefasComponent),
      },
      {
        path: 'pedidos',
        data: { title: 'Pedidos', breadcrumb: 'Comercial' },
        loadComponent: () =>
          import('./features/pedidos/pedidos-list/pedidos-list.component').then((m) => m.PedidosListComponent),
      },
      {
        path: 'pedidos/:id',
        data: { title: 'Detalhe do pedido', breadcrumb: 'Comercial' },
        loadComponent: () =>
          import('./features/pedidos/pedido-detalhe/pedido-detalhe.component').then((m) => m.PedidoDetalheComponent),
      },
      {
        path: 'prestadores',
        data: { title: 'Prestadores', breadcrumb: 'Gestão' },
        loadComponent: () =>
          import('./features/prestadores/prestadores.component').then((m) => m.PrestadoresComponent),
      },
      {
        path: 'veiculos',
        data: { title: 'Veículos', breadcrumb: 'Gestão' },
        loadComponent: () =>
          import('./features/veiculos/veiculos.component').then((m) => m.VeiculosComponent),
      },
      {
        path: 'clientes',
        data: { title: 'Clientes', breadcrumb: 'Gestão' },
        loadComponent: () =>
          import('./features/clientes/clientes.component').then((m) => m.ClientesComponent),
      },
      {
        path: 'financeiro',
        data: { title: 'Financeiro', breadcrumb: 'Gestão' },
        loadComponent: () =>
          import('./features/financeiro/financeiro.component').then((m) => m.FinanceiroComponent),
      },
      {
        path: 'nps',
        data: { title: 'NPS / Avaliações', breadcrumb: 'Gestão' },
        loadComponent: () =>
          import('./features/nps/nps.component').then((m) => m.NpsComponent),
      },
      {
        path: 'relatorios',
        data: { title: 'Relatórios', breadcrumb: 'Gestão' },
        loadComponent: () =>
          import('./features/relatorios/relatorios.component').then((m) => m.RelatoriosComponent),
      },
      {
        path: 'email-integracao',
        data: { title: 'Integração E-mail', breadcrumb: 'Configurações' },
        loadComponent: () =>
          import('./features/email-integracao/email-integracao.component').then((m) => m.EmailIntegracaoComponent),
      },
      {
        path: 'empresa',
        data: { title: 'Empresa', breadcrumb: 'Configurações' },
        loadComponent: () =>
          import('./features/empresa/empresa.component').then((m) => m.EmpresaComponent),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
