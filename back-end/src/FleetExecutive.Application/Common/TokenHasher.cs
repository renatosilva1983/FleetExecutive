using System.Security.Cryptography;
using System.Text;

namespace FleetExecutive.Application.Common;

/// <summary>
/// Hash de tokens opacos de alta entropia (refresh token, token de reset de senha) — SHA-256 é
/// apropriado aqui (diferente de senha: o token já é aleatório/alto-entropia, não precisa de
/// custo computacional propositalmente alto como BCrypt/Argon2, que além disso tem limite de 72
/// bytes de entrada). Nunca reutilizar isto para hash de senha — ver IPasswordHasher.
/// </summary>
public static class TokenHasher
{
    public static string Hash(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }
}
