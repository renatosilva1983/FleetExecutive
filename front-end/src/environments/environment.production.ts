/**
 * Ambiente de produção.
 *
 * Em produção o front-end e a API costumam ser servidos sob o mesmo host
 * (o tenant é resolvido pelo próprio Host da requisição), então `apiBaseUrl`
 * pode permanecer relativo. Caso a API fique em outro domínio, defina a URL
 * absoluta aqui (ex.: 'https://api.fleetexecutive.com.br').
 */
export const environment = {
  production: true,
  apiBaseUrl: '',
  tenantHost: '',
} as const;
