import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, map, startWith } from 'rxjs';
import { AuthService } from '../../core/auth/auth.service';

/** Metadados de rota consumidos pela topbar (definidos em `data` nas rotas). */
interface RouteMeta {
  title: string;
  breadcrumb: string;
}

@Component({
  selector: 'app-topbar',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="topbar">
      <div class="topbar-left">
        <span class="breadcrumb">{{ meta().breadcrumb }} /</span>
        <span class="page-title">{{ meta().title }}</span>
      </div>
      <div class="topbar-right">
        <div class="topbar-icon-btn" title="Mensagens">💌<div class="notif-dot"></div></div>
        <div class="topbar-icon-btn" title="Notificações">🔔<div class="notif-dot"></div></div>
        <div class="user-pill" (click)="logout()" title="Sair">
          <div class="user-avatar">{{ initial() }}<div class="online-dot"></div></div>
          <div class="user-info">
            <div class="name">{{ displayName() }}</div>
            <div class="role">{{ perfil() }}</div>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class TopbarComponent {
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly auth = inject(AuthService);

  protected readonly meta = toSignal(
    this.router.events.pipe(
      filter((e): e is NavigationEnd => e instanceof NavigationEnd),
      startWith(null),
      map(() => this.readDeepestMeta()),
    ),
    { initialValue: { title: 'Painel', breadcrumb: 'Início' } as RouteMeta },
  );

  protected readonly perfil = computed(() => this.auth.user()?.perfil ?? 'Usuário');
  protected readonly displayName = computed(() => {
    const email = this.auth.user()?.email ?? '';
    return email ? email.split('@')[0] : 'Convidado';
  });
  protected readonly initial = computed(() => this.displayName().charAt(0).toUpperCase() || 'U');

  protected logout(): void {
    this.auth.logout();
  }

  private readDeepestMeta(): RouteMeta {
    let r = this.route.root;
    while (r.firstChild) {
      r = r.firstChild;
    }
    const data = r.snapshot.data;
    return {
      title: (data['title'] as string) ?? 'Painel',
      breadcrumb: (data['breadcrumb'] as string) ?? 'Início',
    };
  }
}
