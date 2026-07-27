using FleetExecutive.Application.Common.Interfaces;

namespace FleetExecutive.Infrastructure.Identity;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string senha) => BCrypt.Net.BCrypt.EnhancedHashPassword(senha, workFactor: 12);

    public bool Verify(string senha, string hash) => BCrypt.Net.BCrypt.EnhancedVerify(senha, hash);
}
