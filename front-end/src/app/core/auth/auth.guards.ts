import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';
import { AuthService } from './auth.service';
import { Perfil } from './auth.models';

/** Exige sessão autenticada; caso contrário redireciona para /login guardando o retorno. */
export const authGuard: CanActivateFn = (_route, state): boolean | UrlTree => {
  const auth = inject(AuthService);
  const router = inject(Router);
  if (auth.isAuthenticated()) {
    return true;
  }
  return router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } });
};

/** Bloqueia /login para quem já está autenticado (evita re-login). */
export const guestGuard: CanActivateFn = (): boolean | UrlTree => {
  const auth = inject(AuthService);
  const router = inject(Router);
  return auth.isAuthenticated() ? router.createUrlTree(['/']) : true;
};

/**
 * Fábrica de guarda por perfil (RBAC). Uso em rotas:
 *   canActivate: [authGuard, roleGuard('Administrador', 'Financeiro')]
 */
export function roleGuard(...roles: Array<Perfil | string>): CanActivateFn {
  return (): boolean | UrlTree => {
    const auth = inject(AuthService);
    const router = inject(Router);
    return auth.hasRole(...roles) ? true : router.createUrlTree(['/']);
  };
}
