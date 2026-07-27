using FleetExecutive.Domain.Usuarios;

namespace FleetExecutive.Application.Common.Interfaces;

/// <summary>
/// Gera o JWT de acesso com as claims mínimas (sub, tenant_id, perfil, driver_id/customer_id
/// quando aplicável) — ver Estrutura/07-autenticacao-seguranca-rbac.md. Vida curta (ex.: 15 min);
/// "manter conectado" é resolvido por refresh token de longa duração, não por um access token
/// longo.
/// </summary>
public interface IJwtTokenGenerator
{
    string GerarAccessToken(User user, Guid tenantId);
    string GerarRefreshToken();
}
