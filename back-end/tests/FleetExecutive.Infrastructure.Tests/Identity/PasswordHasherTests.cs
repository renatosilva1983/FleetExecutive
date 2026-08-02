using FleetExecutive.Infrastructure.Identity;

namespace FleetExecutive.Infrastructure.Tests.Identity;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void Hash_NaoRetornaSenhaEmTextoPlano()
    {
        const string senha = "MinhaSenha123!";

        var hash = _hasher.Hash(senha);

        Assert.NotEqual(senha, hash);
    }

    [Fact]
    public void Verify_SenhaCorreta_RetornaTrue()
    {
        const string senha = "MinhaSenha123!";
        var hash = _hasher.Hash(senha);

        Assert.True(_hasher.Verify(senha, hash));
    }

    [Fact]
    public void Verify_SenhaIncorreta_RetornaFalse()
    {
        var hash = _hasher.Hash("MinhaSenha123!");

        Assert.False(_hasher.Verify("SenhaErrada", hash));
    }

    [Fact]
    public void Hash_MesmaSenhaDuasVezes_ProduzHashesDiferentes()
    {
        // BCrypt usa salt aleatório — hashes distintos, ambos válidos.
        const string senha = "MinhaSenha123!";

        var hash1 = _hasher.Hash(senha);
        var hash2 = _hasher.Hash(senha);

        Assert.NotEqual(hash1, hash2);
        Assert.True(_hasher.Verify(senha, hash1));
        Assert.True(_hasher.Verify(senha, hash2));
    }
}
