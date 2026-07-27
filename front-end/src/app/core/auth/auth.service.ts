import { computed, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CurrentUser, LoginRequest, LoginResult, Perfil } from './auth.models';
import { TokenStorageService } from './token-storage.service';

/**
 * Fonte da verdade da autenticação. Expõe o usuário corrente como signal para
 * consumo reativo nos componentes (topbar, guards de UI, etc.).
 *
 * Observação: o backend ainda não expõe endpoint de refresh (AuthController só
 * tem login/forgot/reset/change-password). Quando existir, `refresh()` abaixo é
 * o ponto de extensão — hoje um access token expirado leva ao logout.
 */
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = `${environment.apiBaseUrl}/api/v1/auth`;

  private readonly _user = signal<CurrentUser | null>(null);

  /** Usuário autenticado (ou null). Reativo. */
  readonly user = this._user.asReadonly();
  readonly isAuthenticated = computed(() => this._user() !== null);
  readonly perfil = computed<Perfil | string | null>(() => this._user()?.perfil ?? null);

  constructor(
    private readonly http: HttpClient,
    private readonly tokens: TokenStorageService,
    private readonly router: Router,
  ) {
    this.restoreSession();
  }

  login(request: LoginRequest): Observable<LoginResult> {
    return this.http.post<LoginResult>(`${this.baseUrl}/login`, request).pipe(
      tap((result) => {
        this.tokens.save(result.accessToken, result.refreshToken, request.email);
        this._user.set(this.buildUser(result.accessToken, request.email));
      }),
    );
  }

  logout(redirect = true): void {
    this.tokens.clear();
    this._user.set(null);
    if (redirect) {
      void this.router.navigate(['/login']);
    }
  }

  hasRole(...roles: Array<Perfil | string>): boolean {
    const perfil = this._user()?.perfil;
    return perfil != null && roles.includes(perfil);
  }

  /** Reidrata o usuário a partir do token salvo, se ainda válido. */
  private restoreSession(): void {
    const token = this.tokens.accessToken;
    if (!token || this.tokens.isExpired(token)) {
      if (token) {
        this.tokens.clear();
      }
      return;
    }
    this._user.set(this.buildUser(token, this.tokens.email ?? ''));
  }

  private buildUser(token: string, email: string): CurrentUser | null {
    const claims = this.tokens.decode(token);
    if (!claims) {
      return null;
    }
    return {
      id: claims.sub,
      email,
      perfil: claims.perfil,
      tenantId: claims.tenant_id,
      driverId: claims.driver_id,
      customerId: claims.customer_id,
    };
  }
}
