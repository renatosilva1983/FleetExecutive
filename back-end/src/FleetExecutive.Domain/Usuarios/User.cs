using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Usuarios;

/// <summary>
/// Conta de usuário dentro do banco do tenant (Estrutura/06-modelo-de-dados.md — cada empresa tem
/// seus próprios usuários, já que os bancos são separados por tenant). DriverId/CustomerId
/// vinculam a conta a um Prestador/Cliente quando Perfil é Fornecedor/Cliente (ver
/// Estrutura/08-perfis-e-permissoes.md — acesso "próprio").
/// </summary>
public class User : Entity
{
    public string Nome { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string SenhaHash { get; private set; } = default!;
    public Perfil Perfil { get; private set; }
    public bool Ativo { get; private set; } = true;
    public bool TwoFactorEnabled { get; private set; }
    public DateTimeOffset? UltimoLoginEm { get; private set; }
    public Guid? DriverId { get; private set; }
    public Guid? CustomerId { get; private set; }

    private User() { }

    public static User Criar(string nome, string email, string senhaHash, Perfil perfil,
        Guid? driverId = null, Guid? customerId = null)
    {
        if (perfil == Perfil.Fornecedor && driverId is null)
            throw new InvalidOperationException("Usuário com perfil Fornecedor precisa de um DriverId vinculado.");
        if (perfil == Perfil.Cliente && customerId is null)
            throw new InvalidOperationException("Usuário com perfil Cliente precisa de um CustomerId vinculado.");

        return new User
        {
            Nome = nome,
            Email = email.Trim().ToLowerInvariant(),
            SenhaHash = senhaHash,
            Perfil = perfil,
            DriverId = driverId,
            CustomerId = customerId,
        };
    }

    public void AtualizarSenha(string novoHash)
    {
        SenhaHash = novoHash;
        MarkUpdated(Id);
    }

    public void RegistrarLogin() => UltimoLoginEm = DateTimeOffset.UtcNow;

    public void Desativar() => Ativo = false;
    public void Ativar() => Ativo = true;
}
