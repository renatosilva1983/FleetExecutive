import { HttpParams } from '@angular/common/http';

/**
 * Constrói HttpParams ignorando chaves com valor null/undefined/'' — evita
 * enviar `?status=null` e mantém as URLs limpas nas listagens com filtros.
 */
export function toHttpParams(query: object | undefined): HttpParams {
  let params = new HttpParams();
  if (!query) {
    return params;
  }
  for (const [key, value] of Object.entries(query)) {
    if (value === null || value === undefined || value === '') {
      continue;
    }
    params = params.set(key, String(value));
  }
  return params;
}
