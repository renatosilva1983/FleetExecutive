import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { NotificationService } from '../notifications/notification.service';

/**
 * Tratamento central de erros HTTP:
 *  - 401: sessão inválida/expirada → logout e volta ao login;
 *  - 403: sem permissão para a ação;
 *  - 400/409/422: mensagem de validação do backend (ProblemDetails/`message`);
 *  - 5xx/0: erro genérico de servidor/rede.
 * A mensagem amigável vai para o toast; o erro segue propagando para quem chamou.
 */
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const notify = inject(NotificationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const isAuthEndpoint = req.url.includes('/api/v1/auth/');

      switch (error.status) {
        case 0:
          notify.error('Não foi possível conectar ao servidor. Verifique sua conexão.');
          break;
        case 401:
          if (!isAuthEndpoint) {
            notify.error('Sua sessão expirou. Faça login novamente.');
            auth.logout();
          }
          break;
        case 403:
          notify.error('Você não tem permissão para executar esta ação.');
          break;
        case 400:
        case 409:
        case 422:
          notify.error(extractMessage(error) ?? 'Requisição inválida.');
          break;
        default:
          if (error.status >= 500) {
            notify.error('Ocorreu um erro no servidor. Tente novamente em instantes.');
          }
      }

      return throwError(() => error);
    }),
  );
};

/** Extrai a mensagem de erro do corpo (ProblemDetails, `{ message }` ou `{ errors }`). */
function extractMessage(error: HttpErrorResponse): string | null {
  const body = error.error;
  if (!body) {
    return null;
  }
  if (typeof body === 'string') {
    return body;
  }
  if (typeof body.detail === 'string') {
    return body.detail;
  }
  if (typeof body.message === 'string') {
    return body.message;
  }
  if (body.errors && typeof body.errors === 'object') {
    const first = Object.values(body.errors as Record<string, string[]>)[0];
    if (Array.isArray(first) && first.length) {
      return first[0];
    }
  }
  return null;
}
