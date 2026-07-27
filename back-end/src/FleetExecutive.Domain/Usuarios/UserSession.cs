using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Usuarios;

/// <summary>
/// Sessão de refresh token para "Manter conectado" (Estrutura/07-autenticacao-seguranca-rbac.md).
/// Guarda só o hash do refresh token — nunca o valor puro. Revogável (troca de senha invalida
/// todas as sessões do usuário).
/// </summary>
public class UserSession : Entity
{
    public Guid UserId { get; private set; }
    public string RefreshTokenHash { get; private set; } = default!;
    public DateTimeOffset ExpiraEm { get; private set; }
    public DateTimeOffset? RevogadoEm { get; private set; }

    private UserSession() { }

    public static UserSession Criar(Guid userId, string refreshTokenHash, TimeSpan validade)
    {
        return new UserSession
        {
            UserId = userId,
            RefreshTokenHash = refreshTokenHash,
            ExpiraEm = DateTimeOffset.UtcNow.Add(validade),
        };
    }

    public bool EstaValida => RevogadoEm is null && ExpiraEm > DateTimeOffset.UtcNow;

    public void Revogar() => RevogadoEm = DateTimeOffset.UtcNow;
}
