/** Perfis de acesso (RBAC) — espelham o enum `Perfil` do backend. */
export type Perfil = 'Administrador' | 'Operacional' | 'Financeiro' | 'Fornecedor' | 'Cliente';

/** Corpo do POST /api/v1/auth/login. */
export interface LoginRequest {
  email: string;
  senha: string;
  manterConectado: boolean;
}

/** Resposta do login — espelha `LoginResult` (Usuarios/Dtos/LoginResult.cs). */
export interface LoginResult {
  accessToken: string;
  refreshToken: string;
  expiresInSeconds: number;
  perfil: string;
}

/** Claims relevantes do access token JWT (ver JwtTokenGenerator do backend). */
export interface JwtClaims {
  sub: string;
  tenant_id: string;
  perfil: Perfil | string;
  driver_id?: string;
  customer_id?: string;
  exp: number;
}

/** Identidade do usuário autenticado, derivada do token + e-mail informado no login. */
export interface CurrentUser {
  id: string;
  email: string;
  perfil: Perfil | string;
  tenantId: string;
  driverId?: string;
  customerId?: string;
}
