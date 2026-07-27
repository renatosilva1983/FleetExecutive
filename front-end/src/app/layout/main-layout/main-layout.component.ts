import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { TopbarComponent } from '../topbar/topbar.component';
import { ToastHostComponent } from '../../shared/components/toast-host/toast-host.component';

/**
 * Casca autenticada da aplicação: sidebar + topbar + área de conteúdo roteada.
 * As páginas de feature renderizam apenas o `.content` (sem repetir a moldura).
 */
@Component({
  selector: 'app-main-layout',
  imports: [RouterOutlet, SidebarComponent, TopbarComponent, ToastHostComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="app-shell">
      <app-sidebar />
      <div class="main">
        <app-topbar />
        <div class="content">
          <router-outlet />
        </div>
      </div>
      <app-toast-host />
    </div>
  `,
})
export class MainLayoutComponent {}
