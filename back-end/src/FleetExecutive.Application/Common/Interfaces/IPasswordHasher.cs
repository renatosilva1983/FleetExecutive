namespace FleetExecutive.Application.Common.Interfaces;

/// <summary>
/// Ver Estrutura/07-autenticacao-seguranca-rbac.md — hash de senha com BCrypt (alternativa mais
/// simples ao Argon2id citada no documento). Nunca logar a senha nem o hash em texto claro fora
/// desta implementação.
/// </summary>
public interface IPasswordHasher
{
    string Hash(string senha);
    bool Verify(string senha, string hash);
}
