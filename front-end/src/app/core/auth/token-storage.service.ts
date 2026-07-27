import { Injectable } from '@angular/core';
import { JwtClaims } from './auth.models';

const ACCESS_TOKEN_KEY = 'fx.accessToken';
const REFRESH_TOKEN_KEY = 'fx.refreshToken';
const EMAIL_KEY = 'fx.email';

/**
 * Persistência de tokens. Isolada num serviço próprio para que a estratégia de
 * armazenamento (hoje localStorage) possa ser trocada em um único lugar — por
 * exemplo, para cookies httpOnly quando houver endpoint de refresh no backend.
 */
@Injectable({ providedIn: 'root' })
export class TokenStorageService {
  get accessToken(): string | null {
    return localStorage.getItem(ACCESS_TOKEN_KEY);
  }

  get refreshToken(): string | null {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
  }

  get email(): string | null {
    return localStorage.getItem(EMAIL_KEY);
  }

  save(accessToken: string, refreshToken: string, email: string): void {
    localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
    localStorage.setItem(EMAIL_KEY, email);
    if (refreshToken) {
      localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
    }
  }

  clear(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(EMAIL_KEY);
  }

  /** Decodifica o payload do JWT sem validar assinatura (validação é do backend). */
  decode(token: string): JwtClaims | null {
    try {
      const payload = token.split('.')[1];
      const json = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
      return JSON.parse(decodeURIComponent(escape(json))) as JwtClaims;
    } catch {
      return null;
    }
  }

  isExpired(token: string): boolean {
    const claims = this.decode(token);
    if (!claims?.exp) {
      return true;
    }
    // `exp` é epoch em segundos; margem de 5s para clock skew do cliente.
    return claims.exp * 1000 <= Date.now() + 5000;
  }
}
