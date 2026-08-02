using FleetExecutive.Application.Common;

namespace FleetExecutive.Application.Tests.Common;

public class TokenHasherTests
{
    [Fact]
    public void Hash_MesmoToken_ProduzMesmoHash()
    {
        Assert.Equal(TokenHasher.Hash("token-abc"), TokenHasher.Hash("token-abc"));
    }

    [Fact]
    public void Hash_TokensDiferentes_ProduzemHashesDiferentes()
    {
        Assert.NotEqual(TokenHasher.Hash("token-abc"), TokenHasher.Hash("token-xyz"));
    }

    [Fact]
    public void Hash_NaoRetornaOTokenEmTextoPlano()
    {
        const string token = "token-secreto";

        var hash = TokenHasher.Hash(token);

        Assert.NotEqual(token, hash);
        // SHA-256 em hexadecimal → 64 caracteres.
        Assert.Equal(64, hash.Length);
    }
}
