import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { TokenStorageService } from '../auth/token-storage.service';

/**
 * Anexa o access token (Bearer) às chamadas da API. O tenant é resolvido pelo
 * backend via Host (Finbuckle WithHostStrategy) — em dev o proxy reescreve o
 * Host para localhost:5248 — portanto não enviamos cabeçalho de tenant aqui.
 */
export const authTokenInterceptor: HttpInterceptorFn = (req, next) => {
  // Só intercepta chamadas à nossa API; deixa assets/terceiros passarem intactos.
  const isApiCall = req.url.startsWith('/api') || req.url.includes('/api/v1/');
  if (!isApiCall) {
    return next(req);
  }

  const token = inject(TokenStorageService).accessToken;
  if (!token) {
    return next(req);
  }

  return next(
    req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }),
  );
};
