using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Usuarios;

/// <summary>
/// Token opaco de "esqueci minha senha" (Estrutura/07-autenticacao-seguranca-rbac.md). Guarda só
/// o hash do token, nunca o valor puro — o valor puro só existe no e-mail enviado ao usuário.
/// </summary>
public class PasswordResetToken : Entity
{
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = default!;
    public DateTimeOffset ExpiraEm { get; private set; }
    public DateTimeOffset? UsadoEm { get; private set; }

    private PasswordResetToken() { }

    public static PasswordResetToken Criar(Guid userId, string tokenHash, TimeSpan validade)
    {
        return new PasswordResetToken
        {
            UserId = userId,
            TokenHash = tokenHash,
            ExpiraEm = DateTimeOffset.UtcNow.Add(validade),
        };
    }

    public bool EstaValido => UsadoEm is null && ExpiraEm > DateTimeOffset.UtcNow;

    public void MarcarUsado() => UsadoEm = DateTimeOffset.UtcNow;
}
