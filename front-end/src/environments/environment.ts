/**
 * Ambiente de desenvolvimento (padrão).
 *
 * `apiBaseUrl` fica vazio de propósito: as chamadas usam caminhos relativos
 * (`/api/v1/...`) e o proxy do Angular (proxy.conf.json) as encaminha para a
 * API .NET em http://localhost:5248, evitando problemas de CORS em dev.
 *
 * O backend é multi-tenant por Host (ver Program.cs do FleetExecutive.Api).
 * Em dev usamos um cabeçalho de tenant fixo; ajuste conforme o seed local.
 */
export const environment = {
  production: false,
  apiBaseUrl: '',
  /** Host de tenant enviado em dev quando o backend resolve tenant por header. */
  tenantHost: 'localhost',
} as const;
